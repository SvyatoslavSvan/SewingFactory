using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

public sealed class DeleteProcessViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}