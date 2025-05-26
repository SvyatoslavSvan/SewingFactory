using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.StockItems
{
    public class StockItemReadViewModel
    {
        public required GarmentModelReadViewModel GarmentModel { get; set; }

        public int Quantity { get; set; }
    }
}
