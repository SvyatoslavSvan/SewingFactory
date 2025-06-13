using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentModels.ViewModels;

public sealed class DeleteGarmentModelViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}