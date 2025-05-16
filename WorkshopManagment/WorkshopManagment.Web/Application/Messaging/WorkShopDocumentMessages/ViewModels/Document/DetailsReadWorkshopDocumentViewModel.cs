using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Tasks;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.WorkShopDocumentMessages.ViewModels.Document;

public class DetailsReadWorkshopDocumentViewModel : WorkshopDocumentViewModel, IIdentityViewModel
{
    public List<ReadWorkshopTaskViewModel> WorkshopTasks { get; set; } = null!;
    public Guid Id { get; set; }
}