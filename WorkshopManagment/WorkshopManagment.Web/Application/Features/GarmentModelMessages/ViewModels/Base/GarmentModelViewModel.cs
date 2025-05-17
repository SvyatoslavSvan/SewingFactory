namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels.Base;

public abstract class GarmentModelViewModel
{
    public required string Name { get; set; }
    public byte[]? Image { get; set; }
}