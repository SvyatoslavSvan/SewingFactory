using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;

public class CreateWorkshopDocumentViewModel : WorkshopDocumentViewModel
{
    public Guid GarmentModelId { get; set; }
    public Guid DepartmentId { get; set; }
}