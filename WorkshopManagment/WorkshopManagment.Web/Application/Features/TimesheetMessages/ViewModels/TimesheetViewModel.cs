namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;

public class TimesheetViewModel
{
    public int Hours { get; set; }
    public int DaysCount { get; set; }
    public DateOnly Date { get; set; }
    public List<TimesheetEmployeeViewModel> Employees { get; set; } = null!;
}