namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels.Base;

public abstract class EmployeeViewModel
{
    public string Name { get; set; } = default!;
    public string InternalId { get; set; } = default!;
}