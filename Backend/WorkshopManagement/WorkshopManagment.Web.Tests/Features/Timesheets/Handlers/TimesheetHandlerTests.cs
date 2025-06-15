using AutoMapper;
using Calabonga.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Quartz;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Tests.Common;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Mapping;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Providers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.QuartzJobs;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.Queries;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;
using TimeZoneConverter;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Timesheets.Handlers;

public sealed class TimesheetHandlerTests
{
    private readonly ClaimsPrincipal _user = new(new ClaimsIdentity([
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.Role, "Admin")
    ]));

    private static ServiceProvider BuildProvider(IDateTimeProvider dtProvider, Action<IServiceCollection>? extra = null)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton(dtProvider);
        services.AddDbContext<ApplicationDbContext>(optionsAction: o =>
            o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddUnitOfWork<ApplicationDbContext>();
        services.AddAutoMapper(configAction: (sp, cfg) => cfg.ConstructServicesUsing(sp.GetService),
            typeof(TimesheetMappingProfile).Assembly);

        extra?.Invoke(services);

        return services.BuildServiceProvider();
    }

    private static void SeedEmployees(IUnitOfWork uow, int count = 3)
    {
        var dept = TestHelpers.SeedDepartment((IUnitOfWork<ApplicationDbContext>)uow);
        for (var i = 0; i < count; i++)
        {
            var emp = new RateBasedEmployee($"Emp{i}", $"INT{i:D3}", new Money(100), dept);
            uow.GetRepository<RateBasedEmployee>().Insert(emp);
        }

        uow.SaveChanges();
    }

    [Fact]
    public async Task GenerateMonthlyTimesheet_Should_Create_New_When_Not_Exists()
    {
        // arrange
        var dt = new FixedDateProvider(new DateOnly(2025, 6, 1));
        await using var provider = BuildProvider(dt);
        var uow = provider.GetRequiredService<IUnitOfWork>();
        SeedEmployees(uow);
        var handler = new GenerateMonthlyTimesheetRequestHandler(uow, dt, NullLogger<GenerateMonthlyTimesheetRequestHandler>.Instance);

        // act
        await handler.Handle(new GenerateMonthlyTimesheetRequest(), CancellationToken.None);

        // assert
        var repo = uow.GetRepository<Timesheet>();
        var created = await repo.GetAllAsync(TrackingType.NoTracking);
        ;
        Assert.Single(created);
        Assert.Equal(dt.CurrentMonthStart, created.First().Date);
    }

    [Fact]
    public async Task GenerateMonthlyTimesheet_Should_Not_Create_If_Already_Exists()
    {
        var dt = new FixedDateProvider(new DateOnly(2025, 6, 1));
        await using var provider = BuildProvider(dt);
        var uow = provider.GetRequiredService<IUnitOfWork>();
        SeedEmployees(uow);
        await new GenerateMonthlyTimesheetRequestHandler(uow, dt, NullLogger<GenerateMonthlyTimesheetRequestHandler>.Instance)
            .Handle(new GenerateMonthlyTimesheetRequest(), CancellationToken.None);

        var handler = new GenerateMonthlyTimesheetRequestHandler(uow, dt, NullLogger<GenerateMonthlyTimesheetRequestHandler>.Instance);
        await handler.Handle(new GenerateMonthlyTimesheetRequest(), CancellationToken.None);

        var repo = uow.GetRepository<Timesheet>();
        var all = await repo.GetAllAsync(TrackingType.NoTracking);
        ;
        Assert.Single(all);
    }

    [Fact]
    public async Task ManualCreateTimesheet_Should_Insert_And_Return_ViewModel()
    {
        var dt = new FixedDateProvider(new DateOnly(2025, 6, 1));
        await using var provider = BuildProvider(dt);
        var uow = provider.GetRequiredService<IUnitOfWork>();
        var mapper = provider.GetRequiredService<IMapper>();
        SeedEmployees(uow);
        var handler = new ManualCreateTimesheetRequestHandler(uow, mapper, dt);

        var result = await handler.Handle(new ManualCreateTimesheetRequest(_user), CancellationToken.None);

        Assert.True(result.Ok);
        Assert.NotNull(result.Result);
        Assert.Equal(dt.CurrentMonthStart, result.Result!.Date);
    }

    [Fact]
    public async Task GetCurrentTimesheet_Should_Return_ViewModel_With_Workdays()
    {
        var dt = new FixedDateProvider(new DateOnly(2025, 6, 1));
        await using var provider = BuildProvider(dt);
        var uow = provider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = provider.GetRequiredService<IMapper>();
        SeedEmployees(uow);
        await new GenerateMonthlyTimesheetRequestHandler(uow, dt, NullLogger<GenerateMonthlyTimesheetRequestHandler>.Instance)
            .Handle(new GenerateMonthlyTimesheetRequest(), CancellationToken.None);

        var handler = new GetCurrentTimesheetRequestHandler(uow, mapper, dt);
        var op = await handler.Handle(new GetCurrentTimesheetRequest(_user), CancellationToken.None);

        Assert.True(op.Ok);
        Assert.Equal(3, op.Result!.Employees.Count);
        Assert.All(op.Result.Employees, action: e => Assert.NotEmpty(e.WorkDays));
    }

    [Fact]
    public void CronExpression_Should_Fire_First_Day_Midnight_Kyiv()
    {
        var tz = TZConvert.GetTimeZoneInfo("Europe/Kyiv");
        var cron = new CronExpression("0 0 0 1 * ?") { TimeZone = tz };
        var after = new DateTimeOffset(new DateTime(2025, 5, 15, 12, 0, 0), tz.GetUtcOffset(new DateTime(2025, 5, 15)));
        var next = cron.GetNextValidTimeAfter(after);
        var expected = new DateTimeOffset(new DateTime(2025, 6, 1, 0, 0, 0), tz.GetUtcOffset(new DateTime(2025, 6, 1)));

        Assert.Equal(expected, next);
    }

    [Fact]
    public async Task GenerateMonthlyTimesheetJob_Should_Invoke_Mediator()
    {
        var mediator = new Mock<IMediator>();
        var job = new GenerateMonthlyTimesheetJob(mediator.Object);
        var ctx = new Mock<IJobExecutionContext>();
        await job.Execute(ctx.Object);
        mediator.Verify(expression: m => m.Send(It.IsAny<GenerateMonthlyTimesheetRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private sealed record FixedDateProvider(DateOnly Fixed) : IDateTimeProvider
    {
        public DateOnly CurrentMonthStart => Fixed;
    }
}