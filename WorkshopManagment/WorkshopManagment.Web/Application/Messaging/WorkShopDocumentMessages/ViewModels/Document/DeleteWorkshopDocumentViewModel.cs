using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document;

public class DeleteWorkshopDocumentViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}