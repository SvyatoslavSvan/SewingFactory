using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.Routers;

public sealed class DepartmentRouter : CommandRouter<Department,
    ReadDepartmentViewModel,
    CreateDepartmentViewModel,
    UpdateDepartmentViewModel,
    DeleteDepartmentViewModel,
    ReadDepartmentViewModel>
{
    protected override string PolicyName => AppData.DesignerAccess;
}