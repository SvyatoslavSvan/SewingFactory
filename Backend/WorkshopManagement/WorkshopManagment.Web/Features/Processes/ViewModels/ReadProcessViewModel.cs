using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

public sealed class ReadProcessViewModel : ProcessViewModel, IIdentityViewModel
{
    public required ReadDepartmentViewModel DepartmentViewModel { get; set; }
    public Guid Id { get; set; }
}