namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels.Base;

public abstract class GarmentModelViewModel
{
    public required string Name { get; set; }
    public byte[]? Image { get; set; }
    
    public decimal Price { get; set; }
}