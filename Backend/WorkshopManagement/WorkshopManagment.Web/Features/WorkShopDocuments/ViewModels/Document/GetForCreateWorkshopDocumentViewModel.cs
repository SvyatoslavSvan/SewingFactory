using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;

public class GetForCreateWorkshopDocumentViewModel
{
    public required List<ReadGarmentModelViewModel> GarmentModel { get; set; }
    public required List<ReadDepartmentViewModel> Departments { get; set; }
}