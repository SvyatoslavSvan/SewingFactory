using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Tasks;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;

public sealed class UpdateWorkshopDocumentViewModel : WorkshopDocumentViewModel, IIdentityViewModel
{
    public List<UpdateWorkShopTaskViewModel> WorkshopTasks { get; set; } = null!;
    public Guid Id { get; set; }
}