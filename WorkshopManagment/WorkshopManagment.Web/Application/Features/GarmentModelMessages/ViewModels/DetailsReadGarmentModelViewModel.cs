using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels;

public class DetailsReadGarmentModelViewModel : ReadGarmentModelViewModel
{
    public required List<ReadProcessViewModel> Processes { get; set; }
    public required ReadGarmentCategoryViewModel Category { get; set; }
}