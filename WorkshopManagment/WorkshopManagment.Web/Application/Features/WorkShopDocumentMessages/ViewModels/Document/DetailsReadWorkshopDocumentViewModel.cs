using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Tasks;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.WorkShopDocumentMessages.ViewModels.Document;

public class DetailsReadWorkshopDocumentViewModel : WorkshopDocumentViewModel, IIdentityViewModel
{
    public List<ReadWorkshopTaskViewModel> WorkshopTasks { get; set; } = null!;
    public Guid Id { get; set; }
}