using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.ProcessMessages.ViewModels;

public sealed class DeleteProcessViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}