using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Tests;

/// <summary>
///     Common factory helpers reused by several unit-test classes.
/// </summary>
internal static class TestFixture
{
    /// <summary>
    ///     Produces a PointOfSale with a single GarmentModel already in stock.
    ///     Returns created point-of-sale, model and its stock item.
    /// </summary>
    internal static (PointOfSale pos,
        GarmentModel model,
        StockItem stock) CreatePointOfSale(int initialQty = 10)
    {
        var category = new GarmentCategory("T-Shirts", []);
        var model = new GarmentModel("Classic Tee", category, Money.Zero);

        var pos = new PointOfSale("Main store");
        pos.AddStockItem(model);

        var stockItem = pos.StockItems.First();
        stockItem.IncreaseQuantity(initialQty);

        return (pos, model, stockItem);
    }
}
