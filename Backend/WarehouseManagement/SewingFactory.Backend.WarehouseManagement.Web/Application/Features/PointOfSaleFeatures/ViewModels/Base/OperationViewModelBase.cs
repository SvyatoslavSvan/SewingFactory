namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Base
{
    public abstract class OperationViewModelBase
    {
        public Guid GarmentModelId { get; set; }
        public Guid PointOfSaleId { get; set; }
    }
}
