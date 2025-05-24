namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels.Base;

public abstract class GarmentModelViewModel
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}
