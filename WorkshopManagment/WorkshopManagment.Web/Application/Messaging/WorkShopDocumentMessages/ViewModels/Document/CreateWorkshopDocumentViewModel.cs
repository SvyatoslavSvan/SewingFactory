using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document;

public class CreateWorkshopDocumentViewModel : WorkshopDocumentViewModel
{
    public Guid GarmentModelId { get; set; }
    public Guid DepartmentId { get; set; }
}