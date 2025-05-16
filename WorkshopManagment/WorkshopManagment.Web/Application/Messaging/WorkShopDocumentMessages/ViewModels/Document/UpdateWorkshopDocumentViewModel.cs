using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Tasks;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document;

public class UpdateWorkshopDocumentViewModel : WorkshopDocumentViewModel,IIdentityViewModel
{
    public Guid Id { get; set; }
    public List<UpdateWorkShopTaskViewModel> WorkshopTasks { get; set; } = null!;
}