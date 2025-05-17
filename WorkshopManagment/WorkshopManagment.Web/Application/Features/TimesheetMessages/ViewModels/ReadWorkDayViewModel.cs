using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;

public class ReadWorkDayViewModel : IIdentityViewModel
{
    public DateOnly Date { get; init; }          
    public int Hours { get; init; }
    public Guid Id { get; set; }
}