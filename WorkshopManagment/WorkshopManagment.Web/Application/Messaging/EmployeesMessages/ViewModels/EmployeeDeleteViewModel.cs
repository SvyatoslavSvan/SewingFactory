using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

public sealed class EmployeeDeleteViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}