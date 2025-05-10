using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentModelMessages.ViewModels;

public class DeleteGarmentModelViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}