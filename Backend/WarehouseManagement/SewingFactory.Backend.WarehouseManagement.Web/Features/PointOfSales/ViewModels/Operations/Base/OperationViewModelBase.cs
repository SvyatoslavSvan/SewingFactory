namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Operations.Base;

public abstract class OperationViewModelBase
{
    public Guid GarmentModelId { get; set; }
    public Guid PointOfSaleId { get; set; }
}
