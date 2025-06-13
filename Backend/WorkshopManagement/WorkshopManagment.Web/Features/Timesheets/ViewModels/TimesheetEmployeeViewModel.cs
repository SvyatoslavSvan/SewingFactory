namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Timesheets.ViewModels;

public class TimesheetEmployeeViewModel
{
    public string EmployeeName { get; init; } = null!;
    public List<ReadWorkDayViewModel> WorkDays { get; init; } = [];
}