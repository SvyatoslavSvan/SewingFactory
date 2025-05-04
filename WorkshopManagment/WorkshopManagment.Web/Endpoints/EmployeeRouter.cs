using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints;

public sealed class EmployeeRouter : CommandRouter<Employee, EmployeeReadViewModel, EmployeeCreateViewModel, EmployeeUpdateViewModel, EmployeeDeleteViewModel>
{
}