using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;

public sealed class ReadProcessViewModel : ProcessViewModel, IIdentityViewModel
{
    public required ReadDepartmentViewModel DepartmentViewModel { get; set; }
    public Guid Id { get; set; }
}