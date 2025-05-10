using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;

public class GetForCreateGarmentModelViewModel
{
    public required List<ReadProcessViewModel> Processes { get; set; }
    public required List<ReadGarmentCategoryViewModel> GarmentCategories { get; set; }
}