using AutoMapper;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Infrastructure;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Queries;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;
using SewingFactory.Common.Domain.ValueObjects;
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

        var result = await handler.Handle(new CreateRequest<PointOfSaleCreateViewModel, PointOfSale, PointOfSaleDetailsReadViewModel>(vm, FakeUser), default);

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

    [Fact]
    public async Task GetPointOfSaleById_Returns_Details_With_Stocks_And_Operations()
    {
        //arrange
        var sp = TestHelpers.BuildServices();
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var cat = new GarmentCategory("Tees", []);
        var model = new GarmentModel("Classic", cat, Money.Zero);

        var pos = new PointOfSale("Main");
        pos.AddStockItem(model);
        pos.StockItems.Single().IncreaseQuantity(4);      

        uow.DbContext.AddRange(cat, model, pos);
        await uow.DbContext.SaveChangesAsync();

        await new ReceiveRequestHandler(uow).Handle(
            new ReceiveRequest(
                new CreateOperationViewModel
                {
                    PointOfSaleId = pos.Id,
                    GarmentModelId = model.Id,
                    Quantity = 2,
                    Date = new DateOnly(2025, 5, 10)
                },
                FakeUser), default);

        await new SellRequestRequestHandler(uow).Handle(
            new SellRequest(
                new CreateOperationViewModel
                {
                    PointOfSaleId = pos.Id,
                    GarmentModelId = model.Id,
                    Quantity = 1,
                    Date = new DateOnly(2025, 5, 11)
                },
                FakeUser), default);

        // --- act ---
        var handler = new GetPointOfSaleByIdHandler(uow, sp.GetRequiredService<IMapper>());
        var res = await handler.Handle(new GetPointOfSaleByIdRequest(FakeUser, pos.Id), default);

        // --- assert ---
        Assert.True(res.Ok);

        var vm = res.Result!;
        Assert.Equal("Main", vm.Name);

        Assert.Single(vm.StockItems);
        Assert.Equal(5, vm.StockItems.First().Quantity);      // 4 + 2 – 1

        Assert.Equal(2, vm.Operations.Count);
        Assert.Single(vm.Operations.OfType<ReadReceiveOperationViewModel>());
        Assert.Single(vm.Operations.OfType<ReadSellOperationViewModel>());
    }


    [Fact]
    public async Task ReceiveRequest_Increases_Stock_And_Persists()
    {
        var sp = TestHelpers.BuildServices();
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var cat = new GarmentCategory("Tees", []);
        var model = new GarmentModel("Classic", cat, Money.Zero);

        var pos = new PointOfSale("Main");
        pos.AddStockItem(model);                     // qty = 0
        uow.DbContext.AddRange(cat, model, pos);
        await uow.DbContext.SaveChangesAsync();

        var vm = new CreateOperationViewModel
        {
            PointOfSaleId = pos.Id,
            GarmentModelId = model.Id,
            Quantity = 5,
            Date = new DateOnly(2025, 5, 22)
        };

        var handler = new ReceiveRequestHandler(uow);
        var res = await handler.Handle(new ReceiveRequest(vm, FakeUser), default);

        Assert.True(res.Ok);

        var qty = await uow.DbContext.Set<StockItem>()
            .Where(x => x.PointOfSale.Id == pos.Id && x.GarmentModel.Id == model.Id)
            .Select(x => x.Quantity)
            .SingleAsync();
        Assert.Equal(5, qty);
    }

    [Fact]
    public async Task SellRequest_Decreases_Stock_And_Persists()
    {
        var sp = TestHelpers.BuildServices();
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var cat = new GarmentCategory("Tees", []);
        var model = new GarmentModel("Classic", cat, Money.Zero);

        var pos = new PointOfSale("Main");
        pos.AddStockItem(model);
        pos.StockItems.Single().IncreaseQuantity(10);

        uow.DbContext.AddRange(cat, model, pos);
        await uow.DbContext.SaveChangesAsync();

        var vm = new CreateOperationViewModel
        {
            PointOfSaleId = pos.Id,
            GarmentModelId = model.Id,
            Quantity = 3,
            Date = new DateOnly(2025, 5, 22)
        };

        var handler = new SellRequestRequestHandler(uow);
        var res = await handler.Handle(new SellRequest(vm, FakeUser), default);

        Assert.True(res.Ok);

        var qty = await uow.DbContext.Set<StockItem>()
            .Where(x => x.PointOfSale.Id == pos.Id && x.GarmentModel.Id == model.Id)
            .Select(x => x.Quantity)
            .SingleAsync();
        Assert.Equal(7, qty);
    }

    [Fact]
    public async Task WriteOffRequest_Decreases_Stock_And_Persists()
    {
        var sp = TestHelpers.BuildServices();
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var cat = new GarmentCategory("Tees", []);
        var model = new GarmentModel("Classic", cat, Money.Zero);

        var pos = new PointOfSale("Main");
        pos.AddStockItem(model);
        pos.StockItems.Single().IncreaseQuantity(6);

        uow.DbContext.AddRange(cat, model, pos);
        await uow.DbContext.SaveChangesAsync();

        var vm = new CreateOperationViewModel
        {
            PointOfSaleId = pos.Id,
            GarmentModelId = model.Id,
            Quantity = 2,
            Date = new DateOnly(2025, 5, 22)
        };

        var handler = new WriteOffRequestHandler(uow);
        var res = await handler.Handle(new WriteOffRequest(vm, FakeUser), default);

        Assert.True(res.Ok);

        var qty = await uow.DbContext.Set<StockItem>()
            .Where(x => x.PointOfSale.Id == pos.Id && x.GarmentModel.Id == model.Id)
            .Select(x => x.Quantity)
            .SingleAsync();
        Assert.Equal(4, qty);
    }

    [Fact]
    public async Task InternalTransferRequest_Moves_Stock_Between_Points()
    {
        var sp = TestHelpers.BuildServices();
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var cat = new GarmentCategory("Tees", []);
        var model = new GarmentModel("Classic", cat, Money.Zero);

        var sender = new PointOfSale("Sender");
        sender.AddStockItem(model);
        sender.StockItems.Single().IncreaseQuantity(8);

        var receiver = new PointOfSale("Receiver");
        receiver.AddStockItem(model);

        uow.DbContext.AddRange(cat, model, sender, receiver);
        await uow.DbContext.SaveChangesAsync();

        var vm = new CreateInternalTransferViewModel
        {
            PointOfSaleId = sender.Id,
            ReceiverId = receiver.Id,
            GarmentModelId = model.Id,
            Quantity = 5,
            Date = new DateOnly(2025, 5, 22)
        };

        var handler = new InternalTransferRequestHandler(uow);
        var res = await handler.Handle(new InternalTransferRequest(vm, FakeUser), default);

        Assert.True(res.Ok);

        var senderQty = await uow.DbContext.Set<StockItem>()
            .Where(x => x.PointOfSale.Id == sender.Id && x.GarmentModel.Id == model.Id)
            .Select(x => x.Quantity)
            .SingleAsync();
        var receiverQty = await uow.DbContext.Set<StockItem>()
            .Where(x => x.PointOfSale.Id == receiver.Id && x.GarmentModel.Id == model.Id)
            .Select(x => x.Quantity)
            .SingleAsync();

        Assert.Equal(3, senderQty);
        Assert.Equal(5, receiverQty);
    }

    [Fact]
    public async Task SellRequest_WithInsufficientStock_Records_Shortage()
    {
        var sp = TestHelpers.BuildServices();
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var cat = new GarmentCategory("Tees", []);
        var model = new GarmentModel("Classic", cat, Money.Zero);

        var pos = new PointOfSale("Main");
        pos.AddStockItem(model);
        pos.StockItems.Single().IncreaseQuantity(2);          

        uow.DbContext.AddRange(cat, model, pos);
        await uow.DbContext.SaveChangesAsync();

        var vmSell = new CreateOperationViewModel
        {
            PointOfSaleId = pos.Id,
            GarmentModelId = model.Id,
            Quantity = 5,                               
            Date = new DateOnly(2025, 5, 22)
        };

        var sellHandler = new SellRequestRequestHandler(uow);
        var sellRes = await sellHandler.Handle(new SellRequest(vmSell, FakeUser), default);
        Assert.True(sellRes.Ok);

        var afterSell = await uow.DbContext.Set<StockItem>()
            .Where(x => x.PointOfSale.Id == pos.Id && x.GarmentModel.Id == model.Id)
            .Select(x => new { x.Quantity, x.ShortageQuantity })
            .SingleAsync();

        Assert.Equal(0, afterSell.Quantity);
        Assert.Equal(3, afterSell.ShortageQuantity);          

    }

    [Fact]
    public async Task ReceiveRequest_Clears_Shortage_And_Refills_Quantity()
    {
        var sp = TestHelpers.BuildServices();
        var uow = (UnitOfWork<ApplicationDbContext>)sp.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

        var cat = new GarmentCategory("Tees", []);
        var model = new GarmentModel("Classic", cat, Money.Zero);

        var pos = new PointOfSale("Main");
        pos.AddStockItem(model);
        pos.StockItems.Single().IncreaseQuantity(2);

        uow.DbContext.AddRange(cat, model, pos);
        await uow.DbContext.SaveChangesAsync();

        var vmSell = new CreateOperationViewModel
        {
            PointOfSaleId = pos.Id,
            GarmentModelId = model.Id,
            Quantity = 5,
            Date = new DateOnly(2025, 5, 22)
        };
        await new SellRequestRequestHandler(uow)
            .Handle(new SellRequest(vmSell, FakeUser), default);

        var vmReceive = new CreateOperationViewModel
        {
            PointOfSaleId = pos.Id,
            GarmentModelId = model.Id,
            Quantity = 4,
            Date = new DateOnly(2025, 5, 23)
        };
        var receiveRes = await new ReceiveRequestHandler(uow)
            .Handle(new ReceiveRequest(vmReceive, FakeUser), default);

        Assert.True(receiveRes.Ok);

        var final = await uow.DbContext.Set<StockItem>()
            .Where(x => x.PointOfSale.Id == pos.Id && x.GarmentModel.Id == model.Id)
            .Select(x => new { x.Quantity, x.ShortageQuantity })
            .SingleAsync();

        Assert.Equal(4, final.Quantity);          
        Assert.Equal(0, final.ShortageQuantity);  
    }
}
