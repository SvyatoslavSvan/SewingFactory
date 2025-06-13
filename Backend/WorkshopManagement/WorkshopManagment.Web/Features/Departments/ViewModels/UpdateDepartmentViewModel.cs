using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;

public class UpdateDepartmentViewModel : DepartmentViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}