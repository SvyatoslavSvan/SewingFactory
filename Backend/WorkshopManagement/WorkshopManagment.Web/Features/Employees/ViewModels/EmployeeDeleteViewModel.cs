using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;

public sealed class EmployeeDeleteViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}