using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;

public class DeleteWorkshopDocumentViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}