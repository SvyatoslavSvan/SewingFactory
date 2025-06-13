using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Operations.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Operations;

public class CreateOperationViewModel : OperationViewModelBase
{
    public int Quantity { get; set; }
    public DateOnly Date { get; set; }
}
