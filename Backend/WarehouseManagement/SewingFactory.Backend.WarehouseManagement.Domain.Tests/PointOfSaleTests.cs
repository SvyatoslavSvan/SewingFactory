using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Tests;

/// <summary>
///     Business-logic tests for <see cref="PointOfSale" />.
/// </summary>
public sealed class PointOfSaleTests
{
    [Fact(DisplayName = "Receive increases stock and logs an operation")]
    public void Receive_Increments_Stock_And_Adds_Operation()
    {
        var (pos, model, stock) = TestFixture.CreatePOS(0);

        pos.Receive(model, 5, new DateOnly(2025, 5, 22));

        Assert.Equal(5, stock.Quantity);
        Assert.Single(pos.Operations);
    }

    [Fact(DisplayName = "Sell reduces stock and logs an operation")]
    public void Sell_Reduces_Stock_And_Adds_Operation()
    {
        var (pos, model, stock) = TestFixture.CreatePOS();

        pos.Sell(model.Id, 4, new DateOnly(2025, 5, 22));

        Assert.Equal(6, stock.Quantity);
        Assert.Single(pos.Operations);
        Assert.IsType<SaleOperation>(pos.Operations.Single());
    }

    [Fact(DisplayName = "Sell fails when stock is insufficient")]
    public void Sell_Insufficient_Stock_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePOS(1);

        Assert.Throws<SewingFactoryInvalidOperationException>(
            testCode: () => pos.Sell(model.Id, 5, DateOnly.FromDateTime(DateTime.Today)));
    }

    [Fact(DisplayName = "Write-off reduces stock and logs an operation")]
    public void WriteOff_Reduces_Stock_And_Adds_Operation()
    {
        var (pos, model, stock) = TestFixture.CreatePOS(3);

        pos.WriteOff(model.Id, 2, DateOnly.FromDateTime(DateTime.Today));

        Assert.Equal(1, stock.Quantity);
        Assert.Single(pos.Operations);
        Assert.IsType<WriteOffOperation>(pos.Operations.Single());
    }

    [Fact(DisplayName = "Transfer moves stock, updates both POS and logs correct operations")]
    public void Transfer_Moves_Stock_Between_Points_And_Logs_Operations()
    {
        var (sender, model, senderStock) = TestFixture.CreatePOS(8);
        var receiver = new PointOfSale("Receiver");
        receiver.AddStockItem(model);
        var receiverStock = receiver.StockItems.First();

        sender.Transfer(model, 5, new DateOnly(2025, 5, 22), receiver);

        Assert.Equal(3, senderStock.Quantity); 
        Assert.Equal(5, receiverStock.Quantity); 

        Assert.Single(sender.Operations);
        Assert.IsType<InternalTransferOperation>(sender.Operations.Single());

        Assert.Single(receiver.Operations);
        Assert.IsType<ReceiveOperation>(receiver.Operations.Single());
    }

    [Fact(DisplayName = "AddStockItem throws when the model already exists in POS")]
    public void AddStockItem_DuplicateModel_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePOS();
        Assert.Throws<SewingFactoryInvalidOperationException>(() => pos.AddStockItem(model));
    }

    [Fact(DisplayName = "Sell fails when model is not found in stock")]
    public void Sell_Nonexistent_Model_Throws()
    {
        var (pos, _, _) = TestFixture.CreatePOS();
        var missingId = Guid.NewGuid();

        Assert.Throws<SewingFactoryInvalidOperationException>(
            () => pos.Sell(missingId, 1, new DateOnly(2025, 5, 22)));
    }

    [Fact(DisplayName = "Write-off fails when model is not found in stock")]
    public void WriteOff_Nonexistent_Model_Throws()
    {
        var (pos, _, _) = TestFixture.CreatePOS();
        var missingId = Guid.NewGuid();

        Assert.Throws<SewingFactoryInvalidOperationException>(
            () => pos.WriteOff(missingId, 1, new DateOnly(2025, 5, 22)));
    }

    [Fact(DisplayName = "Write-off fails when stock is insufficient")]
    public void WriteOff_Insufficient_Stock_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePOS(initialQty: 1);
        Assert.Throws<SewingFactoryInvalidOperationException>(
            () => pos.WriteOff(model.Id, 5, new DateOnly(2025, 5, 22)));
    }

    [Fact(DisplayName = "Transfer fails when sender does not have the model")]
    public void Transfer_Nonexistent_Model_Throws()
    {
        var sender = new PointOfSale("Sender");
        var receiver = new PointOfSale("Receiver");

        var fakeCategory = new GarmentCategory("Fake", []);
        var fakeModel = new GarmentModel("Fake Tee", fakeCategory, Money.Zero);

        Assert.Throws<SewingFactoryInvalidOperationException>(
            () => sender.Transfer(fakeModel, 1, new DateOnly(2025, 5, 22), receiver));
    }

    [Fact(DisplayName = "Transfer fails when sender stock is insufficient")]
    public void Transfer_Insufficient_Stock_Throws()
    {
        var (sender, model, _) = TestFixture.CreatePOS(initialQty: 2);
        var receiver = new PointOfSale("Receiver");
        receiver.AddStockItem(model);

        Assert.Throws<SewingFactoryInvalidOperationException>(
            () => sender.Transfer(model, 5, new DateOnly(2025, 5, 22), receiver));
    }
}
