using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints;

public class DepartmentRouter : CommandRouter<Department,
    ReadDepartmentViewModel,
    CreateDepartmentViewModel,
    UpdateDepartmentViewModel,
    DeleteDepartmentViewModel,
    ReadDepartmentViewModel>
{
}