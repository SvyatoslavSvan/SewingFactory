using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints
{
    public class DepartmentEndpoints : CommandRouter<Department,
        ReadDepartmentViewModel,
        CreateDepartmentViewModel,
        UpdateDepartmentViewModel,
        DeleteDepartmentViewModel,
        ReadDepartmentViewModel>
    {

    }
}
