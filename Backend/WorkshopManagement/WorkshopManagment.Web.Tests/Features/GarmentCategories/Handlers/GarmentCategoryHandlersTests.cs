using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Mapping.Profiles;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Mapping.Profiles;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.GarmentCategories.Handlers;

public sealed class GarmentCategoryHandlersTests
{

    private static IServiceProvider BuildServices()
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(o =>
            o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        services.AddUnitOfWork<ApplicationDbContext>();
        services.AddAutoMapper(
            (sp, cfg) => cfg.ConstructServicesUsing(sp.GetService),
            typeof(EmployeeMappingProfile).Assembly,
            typeof(GarmentCategoryMappingProfile).Assembly);

        return services.BuildServiceProvider();
    }

    private readonly IServiceProvider _sp = BuildServices();

    private readonly ClaimsPrincipal _user = new(new ClaimsIdentity([
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.Role, "Admin")
    ]));

    private static void SeedCategories(IUnitOfWork<ApplicationDbContext> uow)
    {
        var repo = uow.GetRepository<SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment.GarmentCategory>();

        repo.Insert(new GarmentCategory("Outerwear"));
        repo.Insert(new GarmentCategory("Sportswear"));
        repo.Insert(new GarmentCategory("Accessories"));

        uow.SaveChanges();
    }

    [Fact]
    public async Task CreateGarmentCategoryHandler_Should_Create_And_Return_Category()
    {
        var uow    = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();

        var model   = new CreateGarmentCategoryViewModel { Name = "Kidswear" };
        var request = new CreateGarmentCategoryRequest(model, _user);
        var handler = new CreateGarmentCategoryHandler(uow, mapper);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result.Result);
        Assert.Equal("Kidswear", result.Result!.Name);

        var saved = await uow.GetRepository<
                SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment.GarmentCategory>()
            .GetFirstOrDefaultAsync(predicate: c => c.Name == "Kidswear");

        Assert.NotNull(saved);
    }

    [Fact]
    public async Task GetByIdGarmentCategoryHandler_Should_Return_Category_When_Found()
    {
        var uow    = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();

        SeedCategories(uow);

        var repo     = uow.GetRepository<
               SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment.GarmentCategory>();
        var existing = await repo.GetFirstOrDefaultAsync(predicate: c => c.Name == "Outerwear");

        Assert.NotNull(existing);

        var request = new GetByIdGarmentCategoryRequest(_user, existing!.Id);
        var handler = new GetByIdGarmentCategoryHandler(uow, mapper);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(result.Result);
        Assert.Equal(existing.Id,   result.Result!.Id);
        Assert.Equal(existing.Name, result.Result.Name);
    }

    [Fact]
    public async Task GetAllGarmentCategoryHandler_Should_Return_All_Categories()
    {
        var uow    = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();

        SeedCategories(uow);

        var request = new GetAllGarmentCategoryRequest(_user);
        var handler = new GetAllGarmentCategoryHandler(uow, mapper);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(3, result.Result!.Count());
    }

    [Fact]
    public async Task GetPagedGarmentCategoryHandler_Should_Return_Paged_Categories()
    {
        var uow    = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();

        SeedCategories(uow);

        var request = new GetPagedGarmentCategoryRequest(_user, 0, 2);
        var handler = new GetPagedGarmentCategoryHandler(uow, mapper);

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(3, result.Result!.TotalCount);
        Assert.Equal(2, result.Result.Items.Count);
    }

    [Fact]
    public async Task UpdateGarmentCategoryHandler_Should_Update_Category()
    {
        var uow    = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();

        SeedCategories(uow);

        var repo = uow.GetRepository<
                   SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment.GarmentCategory>();

        var existing = await repo.GetFirstOrDefaultAsync(
            predicate: c => c.Name == "Sportswear",trackingType: TrackingType.NoTracking);

        Assert.NotNull(existing);

        var updateVm = new UpdateGarmentCategoryViewModel
        {
            Id   = existing!.Id,
            Name = "Activewear"
        };

        uow.DbContext.ChangeTracker.Clear();

        var request = new UpdateGarmentCategoryRequest(updateVm, _user);
        var handler = new UpdateRequestGarmentCategoryHandler(uow, mapper);

        await handler.Handle(request, CancellationToken.None);

        var updated = await repo.GetFirstOrDefaultAsync(
            predicate: c => c.Id == existing.Id, trackingType: TrackingType.NoTracking);

        Assert.Equal("Activewear", updated!.Name);
    }

    [Fact]
    public async Task DeleteGarmentCategoryHandler_Should_Delete_Category()
    {
        var uow = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        SeedCategories(uow);

        uow.DbContext.ChangeTracker.Clear();

        var repo = uow.GetRepository<
                   SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment.GarmentCategory>();

        var victim = await repo.GetFirstOrDefaultAsync(
            predicate: c => c.Name == "Accessories", trackingType: TrackingType.NoTracking);

        Assert.NotNull(victim);

        var delVm  = new DeleteGarmentCategoryViewModel { Id = victim!.Id };
        var request = new DeleteGarmentCategoryRequest(delVm, _user);
        var handler = new DeleteGarmentCategoryHandler(uow);

        await handler.Handle(request, CancellationToken.None);

        var gone = await repo.GetFirstOrDefaultAsync(
            predicate: c => c.Id == victim.Id, trackingType: TrackingType.NoTracking);

        Assert.Null(gone);
        Assert.Equal(2, repo.GetAll(TrackingType.NoTracking).Count());
    }
}