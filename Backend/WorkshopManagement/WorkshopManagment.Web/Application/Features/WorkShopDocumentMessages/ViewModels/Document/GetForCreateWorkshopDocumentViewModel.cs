using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentModelMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;

public class GetForCreateWorkshopDocumentViewModel
{
    public required List<ReadGarmentModelViewModel> GarmentModel { get; set; }
    public required List<ReadDepartmentViewModel> Departments { get; set; }
}