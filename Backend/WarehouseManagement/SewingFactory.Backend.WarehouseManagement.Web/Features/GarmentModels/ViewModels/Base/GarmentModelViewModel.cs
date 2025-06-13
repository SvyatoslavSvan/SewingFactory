namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels.Base;

public abstract class GarmentModelViewModel
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
}
