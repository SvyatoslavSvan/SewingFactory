using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels
{
    public class InternalTransferViewModel : OperationViewModel
    {
        public Guid ReceiverId { get; set; }
    }
}
