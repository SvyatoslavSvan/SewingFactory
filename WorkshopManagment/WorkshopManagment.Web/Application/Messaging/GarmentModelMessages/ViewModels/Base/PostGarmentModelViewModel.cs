namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels.Base;

public abstract class PostGarmentModelViewModel : GarmentModelViewModel
{
    public required string Description { get; set; }
    public required Guid GarmentCategoryId { get; set; }
    public required List<Guid> ProcessesIds { get; set; }
}