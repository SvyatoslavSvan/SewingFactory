using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Tests;

/// <summary>
///     Unit tests covering <see cref="StockItem" /> setters.
/// </summary>
public sealed class StockItemTests
{
    [Fact(DisplayName = "PointOfSale setter rejects null")]
    public void PointOfSale_Null_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePointOfSale();
        var item = new StockItem(1, pos, model);

        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () => item.PointOfSale = null!);
    }

    [Fact(DisplayName = "GarmentModel setter rejects null")]
    public void GarmentModel_Null_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePointOfSale();
        var item = new StockItem(1, pos, model);

        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () => item.GarmentModel = null!);
    }

    [Fact(DisplayName = "ReduceQuantity with sufficient stock decreases quantity and clears shortage")]
    public void ReduceQuantity_Sufficient_Decrements_And_Clears_Shortage()
    {
        var (pos, model, _) = TestFixture.CreatePointOfSale();
        var item = new StockItem(10, pos, model);

        item.ReduceQuantity(4);

        Assert.Equal(6, item.Quantity);
        Assert.Equal(0, item.ShortageQuantity);
    }

    [Fact(DisplayName = "ReduceQuantity with insufficient stock sets shortage and zeroes quantity")]
    public void ReduceQuantity_Insufficient_Sets_Shortage_And_Zero_Quantity()
    {
        var (pos, model, _) = TestFixture.CreatePointOfSale();
        var item = new StockItem(3, pos, model);

        item.ReduceQuantity(5);

        Assert.Equal(0, item.Quantity);
        Assert.Equal(2, item.ShortageQuantity);
    }

    [Fact(DisplayName = "IncreaseQuantity positive value increases quantity and clears shortage")]
    public void IncreaseQuantity_Positive_IncreasesQuantity_And_Clears_Shortage()
    {
        var (pos, model, _) = TestFixture.CreatePointOfSale();
        var item = new StockItem(2, pos, model);

        item.ReduceQuantity(3);
        Assert.Equal(1, item.ShortageQuantity);

        item.IncreaseQuantity(5);

        Assert.Equal(5, item.Quantity);
        Assert.Equal(0, item.ShortageQuantity);
    }

    [Fact(DisplayName = "IncreaseQuantity negative value throws")]
    public void IncreaseQuantity_Negative_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePointOfSale();
        var item = new StockItem(2, pos, model);

        Assert.Throws<SewingFactoryArgumentException>(testCode: () => item.IncreaseQuantity(-1));
    }
}
