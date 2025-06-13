using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Routers;

public class DepartmentRouter : CommandRouter<Department,
    ReadDepartmentViewModel,
    CreateDepartmentViewModel,
    UpdateDepartmentViewModel,
    DeleteDepartmentViewModel,
    ReadDepartmentViewModel>
{
}