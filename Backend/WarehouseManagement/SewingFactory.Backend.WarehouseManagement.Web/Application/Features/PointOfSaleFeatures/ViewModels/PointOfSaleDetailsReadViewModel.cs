using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.StockItems;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels
{
    public sealed class PointOfSaleDetailsReadViewModel : PointOfSaleReadViewModel
    {
        public List<StockItemReadViewModel> StockItems { get; set; }
    }
}
