using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

public sealed class CreateProcessViewModel : ProcessViewModel
{
    public Guid DepartmentId { get; set; }
}