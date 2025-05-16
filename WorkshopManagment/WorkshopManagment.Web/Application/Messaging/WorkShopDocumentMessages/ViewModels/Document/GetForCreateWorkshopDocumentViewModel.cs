using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document;

public class GetForCreateWorkshopDocumentViewModel
{
    public required List<ReadGarmentModelViewModel> GarmentModel { get; set; }
    public required List<ReadDepartmentViewModel> Departments { get; set; }
}