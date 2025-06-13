namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.ViewModels;

public class TimesheetViewModel
{
    public int Hours { get; set; }
    public int DaysCount { get; set; }
    public DateOnly Date { get; set; }
    public List<TimesheetEmployeeViewModel> Employees { get; set; } = null!;
}