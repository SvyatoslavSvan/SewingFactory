using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;

public class UpdateWorkdayViewModel : IIdentityViewModel
{
    public Guid Id { get; set; }
    public int Hours { get; set; }
}