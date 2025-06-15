using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.WorkShopDocuments.ViewModels.Document;

public sealed class ReadWorkshopDocumentViewModel : WorkshopDocumentViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}