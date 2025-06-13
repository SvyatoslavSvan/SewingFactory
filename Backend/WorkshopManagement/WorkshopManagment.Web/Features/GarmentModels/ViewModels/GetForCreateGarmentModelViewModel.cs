using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;

public sealed class GetForCreateGarmentModelViewModel
{
    public required List<ReadProcessViewModel> Processes { get; set; }
    public required List<ReadGarmentCategoryViewModel> GarmentCategories { get; set; }
}