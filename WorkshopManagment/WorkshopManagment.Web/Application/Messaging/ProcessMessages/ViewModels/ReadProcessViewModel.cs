using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.ProcessMessages.ViewModels;

public sealed class ReadProcessViewModel : ProcessViewModel, IIdentityViewModel
{
    public required ReadDepartmentViewModel DepartmentViewModel { get; set; }
    public Guid Id { get; set; }
}