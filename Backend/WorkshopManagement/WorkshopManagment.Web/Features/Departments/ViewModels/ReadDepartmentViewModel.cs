using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;

public class ReadDepartmentViewModel : DepartmentViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}