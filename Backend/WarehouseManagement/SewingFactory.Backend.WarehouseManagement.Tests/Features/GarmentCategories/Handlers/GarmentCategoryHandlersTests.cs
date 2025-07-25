﻿using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Tests.Common;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;
using System.Security.Claims;

namespace SewingFactory.Backend.WarehouseManagement.Tests.Features.GarmentCategories.Handlers;

public sealed class GarmentCategoryHandlersTests
{
    private static ClaimsPrincipal FakeUser => new(new ClaimsIdentity());

    private static async Task<UnitOfWork<ApplicationDbContext>> SeedAsync(IServiceProvider sp)
    {
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
        uow.DbContext.GarmentCategories.Add(new Domain.Entities.Garment.GarmentCategory("T-shirts", new List<Domain.Entities.Garment.GarmentModel>()));
        await uow.DbContext.SaveChangesAsync();

        return uow;
    }

    [Fact]
    public async Task Create_Category_Returns_ReadViewModel()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var handler = new CreateGarmentCategoryHandler(uow, sp.GetRequiredService<IMapper>());
        var vm = new GarmentCategoryCreateViewModel { Name = "Jeans" };

        var result = await handler.Handle(new CreateRequest<GarmentCategoryCreateViewModel, Domain.Entities.Garment.GarmentCategory, GarmentCategoryReadViewModel>(vm, FakeUser), default);

        Assert.True(result.Ok);
        Assert.Equal("Jeans", result.Result!.Name);

        Assert.Equal(2, await uow.DbContext.GarmentCategories.CountAsync());
    }

    [Fact]
    public async Task GetAll_Returns_List()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var handler = new GetAllGarmentCategoriesHandler(uow, sp.GetRequiredService<IMapper>());
        var res = await handler.Handle(new GetAllRequest<Domain.Entities.Garment.GarmentCategory, GarmentCategoryReadViewModel>(FakeUser), default);

        Assert.True(res.Ok);
        Assert.Single(res.Result!);
    }

    [Fact]
    public async Task Update_Changes_Name()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var entity = await uow.DbContext.GarmentCategories.AsNoTracking().FirstAsync();
        var handler = new UpdateGarmentCategoryHandler(uow, sp.GetRequiredService<IMapper>());

        var editVm = new GarmentCategoryEditViewModel { Id = entity.Id, Name = "NewName" };
        var res = await handler.Handle(new UpdateRequest<GarmentCategoryEditViewModel, Domain.Entities.Garment.GarmentCategory>(editVm, FakeUser), default);

        Assert.True(res.Ok);
        Assert.Equal("NewName", res.Result!.Name);
    }

    [Fact]
    public async Task Delete_Removes_Entity()
    {
        var sp = TestHelpers.BuildServices();
        var uow = await SeedAsync(sp);

        var id = await uow.DbContext.GarmentCategories
            .AsNoTracking()
            .Select(selector: x => x.Id)
            .FirstAsync();

        uow.DbContext.ChangeTracker.Clear();

        var handler = new DeleteGarmentCategoryHandler(uow);

        var res = await handler.Handle(
            new DeleteGarmentCategoryRequest(
                new GarmentCategoryDeleteViewModel { Id = id },
                FakeUser),
            CancellationToken.None);

        Assert.True(res.Ok);
        Assert.Empty(uow.DbContext.GarmentCategories);
    }
}
