using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels;

public sealed class UpdateGarmentCategoryViewModel : GarmentCategoryViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}