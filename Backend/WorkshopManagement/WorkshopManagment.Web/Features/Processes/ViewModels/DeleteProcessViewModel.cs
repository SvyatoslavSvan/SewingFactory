using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Processes.ViewModels;

public sealed class DeleteProcessViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
}