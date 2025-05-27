using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.StockItems;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels
{
    public sealed class PointOfSaleDetailsReadViewModel : PointOfSaleReadViewModel
    {
        public required List<StockItemReadViewModel> StockItems { get; set; }

        public required List<ReadOperationViewModel> Operations { get; set; }
    }
}
