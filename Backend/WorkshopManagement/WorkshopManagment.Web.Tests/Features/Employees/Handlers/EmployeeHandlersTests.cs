using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Infrastructure;
using SewingFactory.Backend.WorkshopManagement.Tests.Common;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.Queries;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Employees.Handlers;

public sealed class EmployeeHandlersTests
{
    private readonly IServiceProvider _sp = TestHelpers.BuildServices();

    private readonly ClaimsPrincipal _user = new(new ClaimsIdentity(
    [
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.Role, "Admin")
    ]));

    private static Department SeedDepartment(IUnitOfWork<ApplicationDbContext> uow, string name = "Default Dept")
    {
        var dep = new Department(name);
        uow.GetRepository<Department>().Insert(dep);
        uow.SaveChanges();

        return dep;
    }

    private static void SeedEmployees(IUnitOfWork<ApplicationDbContext> uow, Department dep)
    {
        var repo = uow.GetRepository<Employee>();

        repo.Insert(new ProcessBasedEmployee("Alice", "E-001", dep) { Premium = new Percent(10) });
        repo.Insert(new RateBasedEmployee("Bob", "E-002", new Money(1000), dep, 5));
        repo.Insert(new Technologist("Charlie", "E-003", new Percent(15), dep));

        uow.SaveChanges();
    }


    [Fact]
    public async Task CreateEmployeeHandler_Should_Create_And_Return_Employee()
    {
        // arrange
        var uow = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();
        var dep = SeedDepartment(uow);
        uow.DbContext.ChangeTracker.Clear();

        var model = new ProcessEmployeeCreateViewModel { Name = "New Guy", InternalId = "NG-007", DepartmentId = dep.Id, Premium = 12m };
        var req = new CreateEmployeeRequest(model, _user);
        var handler = new CreateEmployeeHandler(uow, mapper);

        // act
        var res = await handler.Handle(req, CancellationToken.None);

        // assert
        Assert.NotNull(res.Result);
        Assert.Equal(model.Name, res.Result!.Name);

        var saved = await uow.GetRepository<Employee>()
            .GetFirstOrDefaultAsync(predicate: e => e.InternalId == "NG-007", include: q => q.Include(navigationPropertyPath: x => x.Department));

        Assert.NotNull(saved);
        Assert.Equal(dep.Id, saved!.Department.Id);
    }

    [Fact]
    public async Task GetByIdEmployeeHandler_Should_Return_Employee_When_Found()
    {
        var uow = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();
        var dep = SeedDepartment(uow);
        SeedEmployees(uow, dep);

        var existing = await uow.GetRepository<Employee>()
            .GetFirstOrDefaultAsync(predicate: e => e.InternalId == "E-001");

        Assert.NotNull(existing);

        var req = new GetByIdEmployeeRequest(_user, existing!.Id);
        var handler = new GetByIdRequestEmployeeHandler(uow, mapper);

        var res = await handler.Handle(req, CancellationToken.None);

        Assert.NotNull(res.Result);
        Assert.Equal(existing.Id, res.Result!.Id);
        Assert.Equal(existing.Name, res.Result.Name);
    }

    [Fact]
    public async Task GetAllEmployeesHandler_Should_Return_All_Employees()
    {
        var uow = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();
        var dep = SeedDepartment(uow);
        SeedEmployees(uow, dep);

        var req = new GetAllEmployeesRequest(_user);
        var handler = new GetAllRequestEmployeesHandler(uow, mapper);

        var res = await handler.Handle(req, CancellationToken.None);

        Assert.Equal(3, res.Result!.Count());
    }

    [Fact]
    public async Task GetPagedEmployeesHandler_Should_Return_Paged_Employees()
    {
        var uow = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();
        var dep = SeedDepartment(uow);
        SeedEmployees(uow, dep);

        var req = new GetPagedEmployeesRequest(_user, 0, 2);
        var handler = new GetPagedRequestEmployeesHandler(uow, mapper);

        var res = await handler.Handle(req, CancellationToken.None);

        Assert.Equal(3, res.Result!.TotalCount);
        Assert.Equal(2, res.Result.Items.Count);
    }

    [Fact]
    public async Task UpdateEmployeeHandler_Should_Update_Employee()
    {
        var uow = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var mapper = _sp.GetRequiredService<IMapper>();
        var dep = SeedDepartment(uow);
        SeedEmployees(uow, dep);

        var repo = uow.GetRepository<Employee>();
        var employee = await repo.GetFirstOrDefaultAsync(predicate: e => e.InternalId == "E-001",
            trackingType: TrackingType.NoTracking);

        Assert.NotNull(employee);

        var updateVm = new ProcessEmployeeUpdateViewModel
        {
            Id = employee!.Id,
            Name = "Alice Updated",
            InternalId = employee.InternalId,
            DepartmentId = dep.Id,
            Premium = 20m
        };

        uow.DbContext.ChangeTracker.Clear();
        var req = new UpdateProcessBasedEmployeeRequest(updateVm, _user);
        var handler = new UpdateRequestProcessBasedEmployeeHandler(uow, mapper);

        await handler.Handle(req, CancellationToken.None);

        var updated = await repo.GetFirstOrDefaultAsync(predicate: e => e.Id == employee.Id,
            trackingType: TrackingType.NoTracking);

        Assert.Equal("Alice Updated", updated!.Name);
        Assert.Equal(20, ((ProcessBasedEmployee)updated).Premium.Value);
    }

    [Fact]
    public async Task DeleteEmployeeHandler_Should_Delete_Employee()
    {
        var uow = _sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        var dep = SeedDepartment(uow);
        SeedEmployees(uow, dep);

        uow.DbContext.ChangeTracker.Clear();

        var repo = uow.GetRepository<Employee>();
        var victim = await repo.GetFirstOrDefaultAsync(predicate: e => e.InternalId == "E-003",
            trackingType: TrackingType.NoTracking);

        Assert.NotNull(victim);

        var delVm = new EmployeeDeleteViewModel { Id = victim!.Id };
        var req = new DeleteEmployeeRequest(delVm, _user);
        var handler = new DeleteRequestEmployeeHandler(uow);

        await handler.Handle(req, CancellationToken.None);

        var gone = await repo.GetFirstOrDefaultAsync(predicate: e => e.Id == victim.Id,
            trackingType: TrackingType.NoTracking);

        Assert.Null(gone);
        Assert.Equal(2, repo.GetAll(TrackingType.NoTracking).Count());
    }
}