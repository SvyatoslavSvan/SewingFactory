namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels.Base;

public abstract class PostGarmentModelViewModel : GarmentModelViewModel
{
    public required string Description { get; set; }
    public required Guid GarmentCategoryId { get; set; }
    public required List<Guid> ProcessesIds { get; set; }
}