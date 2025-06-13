using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.StockItems;

public class StockItemReadViewModel
{
    public required GarmentModelReadViewModel GarmentModel { get; set; }

    public int Quantity { get; set; }
}
