using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;

public class UpdateDepartmentViewModel : DepartmentViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}