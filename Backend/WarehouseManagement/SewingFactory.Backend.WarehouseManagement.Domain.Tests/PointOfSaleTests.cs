using SewingFactory.Backend.WarehouseManagement.Domain.Entities;

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
        Assert.Equal(OperationType.Receive, pos.Operations.Single().OperationType);
    }

    [Fact(DisplayName = "Sell reduces stock and logs an operation")]
    public void Sell_Reduces_Stock_And_Adds_Operation()
    {
        var (pos, model, stock) = TestFixture.CreatePOS();

        pos.Sell(model, 4, new DateOnly(2025, 5, 22));

        Assert.Equal(6, stock.Quantity);
        Assert.Single(pos.Operations);
        Assert.Equal(OperationType.Sale, pos.Operations.Single().OperationType);
    }

    [Fact(DisplayName = "Sell fails when stock is insufficient")]
    public void Sell_Insufficient_Stock_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePOS(1);

        Assert.Throws<InvalidOperationException>(
            testCode: () => pos.Sell(model, 5, DateOnly.FromDateTime(DateTime.Today)));
    }

    [Fact(DisplayName = "Write-off reduces stock and logs an operation")]
    public void WriteOff_Reduces_Stock_And_Adds_Operation()
    {
        var (pos, model, stock) = TestFixture.CreatePOS(3);

        pos.WriteOff(model, 2, DateOnly.FromDateTime(DateTime.Today));

        Assert.Equal(1, stock.Quantity);
        Assert.Single(pos.Operations);
        Assert.Equal(OperationType.WriteOff, pos.Operations.Single().OperationType);
    }

    [Fact(DisplayName = "Transfer moves stock, updates both POS and logs correct operations")]
    public void Transfer_Moves_Stock_Between_Points_And_Logs_Operations()
    {
        var (sender, model, senderStock) = TestFixture.CreatePOS(8);
        var receiver = new PointOfSale("Receiver");
        receiver.AddStockItem(model);
        var receiverStock = receiver.StockItems.First();

        sender.Transfer(model, 5, new DateOnly(2025, 5, 22), receiver);

        Assert.Equal(3, senderStock.Quantity); // sender decreased
        Assert.Equal(5, receiverStock.Quantity); // receiver increased

        Assert.Single(sender.Operations);
        Assert.Equal(OperationType.InternalTransfer, sender.Operations.Single().OperationType);

        Assert.Single(receiver.Operations);
        Assert.Equal(OperationType.Receive, receiver.Operations.Single().OperationType);
    }
}
