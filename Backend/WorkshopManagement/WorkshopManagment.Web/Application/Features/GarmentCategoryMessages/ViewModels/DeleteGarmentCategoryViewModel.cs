using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.GarmentCategoryMessages.ViewModels;

public sealed class DeleteGarmentCategoryViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}