using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.ViewModels;

public class UpdateWorkdayViewModel : IIdentityViewModel
{
    public int Hours { get; set; }
    public Guid Id { get; set; }
}