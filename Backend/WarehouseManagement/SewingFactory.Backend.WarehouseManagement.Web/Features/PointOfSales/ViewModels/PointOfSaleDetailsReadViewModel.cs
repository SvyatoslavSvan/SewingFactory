using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Operations;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.StockItems;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;

public sealed class PointOfSaleDetailsReadViewModel : PointOfSaleReadViewModel
{
    public required List<StockItemReadViewModel> StockItems { get; set; }

    public required List<ReadOperationViewModel> Operations { get; set; }
}
