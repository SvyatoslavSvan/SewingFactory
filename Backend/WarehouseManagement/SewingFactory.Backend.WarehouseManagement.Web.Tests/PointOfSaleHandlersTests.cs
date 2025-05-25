using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Web.Tests;

public sealed class PointOfSaleHandlersTests
{
    private static ClaimsPrincipal FakeUser => new(new ClaimsIdentity());

    private static async Task<UnitOfWork<ApplicationDbContext>> SeedAsync(IServiceProvider sp)
    {
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        uow.DbContext.Set<PointOfSale>().Add(new PointOfSale("Main Store"));
        await uow.DbContext.SaveChangesAsync();

        return uow;
    }

    [Fact]
    public async Task Create_PointOfSale_Returns_ReadViewModel()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var handler = new CreatePointOfSaleHandler(uow, sp.GetRequiredService<IMapper>());
        var vm = new PointOfSaleCreateViewModel { Name = "Outlet" };

        var result = await handler.Handle(new CreateRequest<PointOfSaleCreateViewModel, PointOfSale, PointOfSaleReadViewModel>(vm, FakeUser), default);

        Assert.True(result.Ok);
        Assert.Equal("Outlet", result.Result!.Name);
        Assert.Equal(2, await uow.DbContext.Set<PointOfSale>().CountAsync());
    }

    [Fact]
    public async Task GetAll_Returns_List()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var handler = new GetAllPointOfSalesHandler(uow, sp.GetRequiredService<IMapper>());
        var res = await handler.Handle(new GetAllRequest<PointOfSale, PointOfSaleReadViewModel>(FakeUser), default);

        Assert.True(res.Ok);
        Assert.Single(res.Result!);
    }

    [Fact]
    public async Task Update_Changes_Name()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var entity = await uow.DbContext.Set<PointOfSale>()
            .AsNoTracking()
            .FirstAsync();

        var handler = new UpdatePointOfSaleHandler(uow, sp.GetRequiredService<IMapper>());

        var editVm = new PointOfSaleEditViewModel { Id = entity.Id, Name = "Flagship" };
        var res = await handler.Handle(new UpdateRequest<PointOfSaleEditViewModel, PointOfSale>(editVm, FakeUser), default);

        Assert.True(res.Ok);
        Assert.Equal("Flagship", res.Result!.Name);
    }

    [Fact]
    public async Task Delete_Removes_Entity()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var id = await uow.DbContext.Set<PointOfSale>()
            .AsNoTracking()
            .Select(selector: x => x.Id)
            .FirstAsync();

        uow.DbContext.ChangeTracker.Clear();

        var handler = new DeletePointOfSaleHandler(uow);
        var res = await handler.Handle(
            new DeletePointOfSaleRequest(
                new PointOfSaleDeleteViewModel { Id = id },
                FakeUser),
            CancellationToken.None);

        Assert.True(res.Ok);
        Assert.Empty(uow.DbContext.Set<PointOfSale>());
    }
}
