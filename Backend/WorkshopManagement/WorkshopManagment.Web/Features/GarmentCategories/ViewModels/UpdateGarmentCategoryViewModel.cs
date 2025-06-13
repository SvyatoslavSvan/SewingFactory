using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;

public sealed class UpdateGarmentCategoryViewModel : GarmentCategoryViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}