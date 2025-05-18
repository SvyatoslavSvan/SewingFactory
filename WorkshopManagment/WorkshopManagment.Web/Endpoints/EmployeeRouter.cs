using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints;

public sealed class EmployeeRouter : CommandRouter<Employee, EmployeeReadViewModel, EmployeeCreateViewModel, EmployeeUpdateViewModel, EmployeeDeleteViewModel, EmployeeReadViewModel>
{
}