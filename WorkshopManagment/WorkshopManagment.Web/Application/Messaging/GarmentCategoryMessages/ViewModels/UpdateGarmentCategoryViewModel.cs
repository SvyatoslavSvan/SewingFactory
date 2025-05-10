using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.ViewModels;

public sealed class UpdateGarmentCategoryViewModel : GarmentCategoryViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}