using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Tests;

/// <summary>
///     Unit tests covering <see cref="StockItem" /> setters.
/// </summary>
public sealed class StockItemTests
{
    [Fact(DisplayName = "Quantity setter rejects negative values")]
    public void Quantity_Negative_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePOS();
        var item = new StockItem(1, pos, model);

        Assert.Throws<SewingFactoryArgumentException>(testCode: () => item.Quantity = -1);
    }

    [Fact(DisplayName = "PointOfSale setter rejects null")]
    public void PointOfSale_Null_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePOS();
        var item = new StockItem(1, pos, model);

        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () => item.PointOfSale = null!);
    }

    [Fact(DisplayName = "GarmentModel setter rejects null")]
    public void GarmentModel_Null_Throws()
    {
        var (pos, model, _) = TestFixture.CreatePOS();
        var item = new StockItem(1, pos, model);

        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () => item.GarmentModel = null!);
    }
}