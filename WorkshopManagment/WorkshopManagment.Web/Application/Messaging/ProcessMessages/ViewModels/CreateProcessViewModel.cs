using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

public sealed class CreateProcessViewModel : ProcessViewModel
{
    public Guid DepartmentId { get; set; }
}