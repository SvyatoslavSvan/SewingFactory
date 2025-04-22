using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

public class IdentityProcessBasedEmployeeViewModel : IdentityViewModel
{
    public ProcessBasedEmployeeViewModel Employee { get; set; } = new();
}