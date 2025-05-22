using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;

public sealed class EmployeeDeleteViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}