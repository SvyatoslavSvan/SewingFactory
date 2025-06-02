using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Operations;

public class CreateOperationViewModel : OperationViewModelBase
{
    public int Quantity { get; set; }
    public DateOnly Date { get; set; }
}
