using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Tests.Common;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;
using SewingFactory.Common.Domain.ValueObjects;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Tests.Features.GarmentModels.Handlers;

public sealed class GarmentModelHandlersTests
{
    private static ClaimsPrincipal Fake => new(new ClaimsIdentity());

    private static async Task<UnitOfWork<ApplicationDbContext>> SeedAsync(IServiceProvider sp)
    {
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var cat = new WarehouseManagement.Domain.Entities.Garment.GarmentCategory("T-shirts", []);
        uow.DbContext.GarmentCategories.Add(cat);

        var gm = new WarehouseManagement.Domain.Entities.Garment.GarmentModel("Basic Tee", cat, Money.Zero);
        uow.DbContext.GarmentModels.Add(gm);

        await uow.DbContext.SaveChangesAsync();

        return uow;
    }

    [Fact]
    public async Task Create_Model_Returns_ReadViewModel()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var catId = await uow.DbContext.GarmentCategories.Select(selector: x => x.Id).FirstAsync();

        var createVm = new GarmentModelCreateViewModel { Name = "Jeans-Slim", CategoryId = catId };

        var handler = new CreateGarmentModelHandler(uow, sp.GetRequiredService<IMapper>());

        var res = await handler.Handle(new CreateRequest<GarmentModelCreateViewModel, WarehouseManagement.Domain.Entities.Garment.GarmentModel, GarmentModelReadViewModel>(createVm, Fake), default);

        Assert.True(res.Ok);
        Assert.Equal("Jeans-Slim", res.Result!.Name);
        Assert.Equal(2, await uow.DbContext.GarmentModels.CountAsync());
    }

    [Fact]
    public async Task GetById_Returns_Model()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var id = await uow.DbContext.GarmentModels.Select(selector: x => x.Id).FirstAsync();
        var handler = new GetGarmentModelByIdHandler(uow, sp.GetRequiredService<IMapper>());

        var res = await handler.Handle(new GetByIdRequest<WarehouseManagement.Domain.Entities.Garment.GarmentModel, GarmentModelReadViewModel>(Fake, id), default);

        Assert.True(res.Ok);
        Assert.Equal(id, res.Result!.Id);
    }

    [Fact]
    public async Task Update_Changes_Name()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var model = await uow.DbContext.GarmentModels.AsNoTracking().Include(navigationPropertyPath: garmentModel => garmentModel.Category).FirstAsync();
        var editVm = new GarmentModelEditViewModel { Id = model.Id, Name = "Basic Tee V2", CategoryId = model.Category.Id };

        var handler = new UpdateGarmentModelHandler(uow, sp.GetRequiredService<IMapper>());
        var res = await handler.Handle(new UpdateRequest<GarmentModelEditViewModel, WarehouseManagement.Domain.Entities.Garment.GarmentModel>(editVm, Fake), default);

        Assert.True(res.Ok);
        var reloaded = await uow.DbContext.GarmentModels.FindAsync(model.Id);
        Assert.Equal("Basic Tee V2", reloaded!.Name);
    }

    [Fact]
    public async Task Delete_Removes_Model()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var id = await uow.DbContext.GarmentModels.Select(selector: x => x.Id).FirstAsync();
        uow.DbContext.ChangeTracker.Clear(); // важливо!

        var delVm = new GarmentModelDeleteViewModel { Id = id };
        var handler = new DeleteGarmentModelHandler(uow);

        var res = await handler.Handle(new DeleteRequest<GarmentModelDeleteViewModel, WarehouseManagement.Domain.Entities.Garment.GarmentModel>(delVm, Fake), default);

        Assert.True(res.Ok);
        Assert.Empty(uow.DbContext.GarmentModels);
    }
}
