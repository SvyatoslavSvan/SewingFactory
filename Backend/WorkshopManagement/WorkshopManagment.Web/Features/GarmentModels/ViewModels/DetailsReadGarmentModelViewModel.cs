using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;

public sealed class DetailsReadGarmentModelViewModel : ReadGarmentModelViewModel
{
    public required List<ReadProcessViewModel> Processes { get; set; }
    public required ReadGarmentCategoryViewModel Category { get; set; }
}