using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;

public class UpdateDepartmentViewModel : DepartmentViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}