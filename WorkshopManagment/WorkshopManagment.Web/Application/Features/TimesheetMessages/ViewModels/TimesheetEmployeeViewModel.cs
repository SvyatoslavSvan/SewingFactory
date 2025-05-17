namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.TimesheetMessages.ViewModels;

public class TimesheetEmployeeViewModel
{
    public string EmployeeName { get; init; } = null!;
    public List<ReadWorkDayViewModel> WorkDays { get; init; } = [];
}