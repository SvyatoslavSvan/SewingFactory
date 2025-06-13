using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;

public sealed class DeleteGarmentCategoryViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}