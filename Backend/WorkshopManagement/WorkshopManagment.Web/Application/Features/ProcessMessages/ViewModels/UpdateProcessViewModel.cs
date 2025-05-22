using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;

public sealed class UpdateProcessViewModel : ProcessViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}