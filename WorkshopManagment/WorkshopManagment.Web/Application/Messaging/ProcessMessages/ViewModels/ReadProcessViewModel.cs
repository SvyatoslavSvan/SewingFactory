using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

public sealed class ReadProcessViewModel : ProcessViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}