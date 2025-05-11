using SewingFactory.Backend.WorkshopManagement.Domain.Enums;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels.Base;

public abstract class EmployeeViewModel
{
    public string Name { get; set; } = default!;
    public string InternalId { get; set; } = default!;
}