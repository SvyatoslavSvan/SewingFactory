using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;

public class DetailsReadGarmentModelViewModel : ReadGarmentModelViewModel
{
    public required List<ReadProcessViewModel> Processes { get; set; }
    public required ReadGarmentCategoryViewModel Category { get; set; }
}