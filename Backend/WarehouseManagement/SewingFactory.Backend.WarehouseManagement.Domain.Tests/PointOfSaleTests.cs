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
        var (pos, model, stock) = TestFixture.CreatePointOfSale(0);

        pos.Receive(model, 5, new DateOnly(2025, 5, 22));

        Assert.Equal(5, stock.Quantity);
        Assert.Single(pos.Operations);
    }

    [Fact(DisplayName = "Sell reduces stock and logs an operation")]
    public void Sell_Reduces_Stock_And_Adds_Operation()
    {
        var (pos, model, stock) = TestFixture.CreatePointOfSale();

        pos.Sell(model.Id, 4, new DateOnly(2025, 5, 22));

        Assert.Equal(6, stock.Quantity);
        Assert.Single(pos.Operations);
        Assert.IsType<SaleOperation>(pos.Operations.Single());
    }

    [Fact(DisplayName = "Sell with insufficient stock records shortage and zeroes qty")]
    public void Sell_Insufficient_Stock_Records_Shortage()
    {
        var (pos, model, stock) = TestFixture.CreatePointOfSale(1);

        pos.Sell(model.Id, 5, new DateOnly(2025, 5, 22));

        Assert.Equal(0, stock.Quantity);
        Assert.Equal(4, stock.ShortageQuantity);
        Assert.Single(pos.Operations);
        Assert.IsType<SaleOperation>(pos.Operations.Single());
    }

    [Fact(DisplayName = "Write-off reduces stock and logs an operation")]
    public void WriteOff_Reduces_Stock_And_Adds_Operation()
    {
        var (pos, model, stock) = TestFixture.CreatePointOfSale(3);

        pos.WriteOff(model.Id, 2, DateOnly.FromDateTime(DateTime.Today));

        Assert.Equal(1, stock.Quantity);
        Assert.Single(pos.Operations);
        Assert.IsType<WriteOffOperation>(pos.Operations.Single());
    }

    [Fact(DisplayName = "Transfer moves stock, updates both POS and logs correct operations")]
    public void Transfer_Moves_Stock_Between_Points_And_Logs_Operations()
    {
        var (sender, model, senderStock) = TestFixture.CreatePointOfSale(8);
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
        var (pos, model, _) = TestFixture.CreatePointOfSale();
        Assert.Throws<SewingFactoryInvalidOperationException>(testCode: () => pos.AddStockItem(model));
    }

    [Fact(DisplayName = "Sell fails when model is not found in stock")]
    public void Sell_Nonexistent_Model_Throws()
    {
        var (pos, _, _) = TestFixture.CreatePointOfSale();
        var missingId = Guid.NewGuid();

        Assert.Throws<SewingFactoryInvalidOperationException>(
            testCode: () => pos.Sell(missingId, 1, new DateOnly(2025, 5, 22)));
    }

    [Fact(DisplayName = "Write-off fails when model is not found in stock")]
    public void WriteOff_Nonexistent_Model_Throws()
    {
        var (pos, _, _) = TestFixture.CreatePointOfSale();
        var missingId = Guid.NewGuid();

        Assert.Throws<SewingFactoryInvalidOperationException>(
            testCode: () => pos.WriteOff(missingId, 1, new DateOnly(2025, 5, 22)));
    }

    [Fact(DisplayName = "Write-off with insufficient stock records shortage and zeroes qty")]
    public void WriteOff_Insufficient_Stock_Records_Shortage()
    {
        var (pos, model, stock) = TestFixture.CreatePointOfSale(1);

        pos.WriteOff(model.Id, 5, new DateOnly(2025, 5, 22));

        Assert.Equal(0, stock.Quantity);
        Assert.Equal(4, stock.ShortageQuantity);
        Assert.Single(pos.Operations);
        Assert.IsType<WriteOffOperation>(pos.Operations.Single());
    }

    [Fact(DisplayName = "Transfer fails when sender does not have the model")]
    public void Transfer_Nonexistent_Model_Throws()
    {
        var sender = new PointOfSale("Sender");
        var receiver = new PointOfSale("Receiver");

        var fakeCategory = new GarmentCategory("Fake", []);
        var fakeModel = new GarmentModel("Fake Tee", fakeCategory, Money.Zero);

        Assert.Throws<SewingFactoryInvalidOperationException>(
            testCode: () => sender.Transfer(fakeModel, 1, new DateOnly(2025, 5, 22), receiver));
    }

    [Fact(DisplayName = "Transfer with insufficient stock records shortage on sender, delivers to receiver")]
    public void Transfer_Insufficient_Stock_Records_Shortage()
    {
        var (sender, model, senderStock) = TestFixture.CreatePointOfSale(2);
        var receiver = new PointOfSale("Receiver");
        receiver.AddStockItem(model);
        var receiverStock = receiver.StockItems.First();

        sender.Transfer(model, 5, new DateOnly(2025, 5, 22), receiver);

        Assert.Equal(0, senderStock.Quantity);
        Assert.Equal(3, senderStock.ShortageQuantity);
        Assert.Equal(5, receiverStock.Quantity);
        Assert.Equal(0, receiverStock.ShortageQuantity);

        Assert.Single(sender.Operations);
        Assert.IsType<InternalTransferOperation>(sender.Operations.Single());
        Assert.Single(receiver.Operations);
        Assert.IsType<ReceiveOperation>(receiver.Operations.Single());
    }
}
