using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations
{
    public class OperationViewModel : OperationViewModelBase
    {
        public int Quantity { get; set; }
        public DateOnly Date { get; set; }
    }
}
