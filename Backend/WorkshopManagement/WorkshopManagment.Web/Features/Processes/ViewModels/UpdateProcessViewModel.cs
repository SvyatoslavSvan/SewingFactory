using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

public sealed class UpdateProcessViewModel : ProcessViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}