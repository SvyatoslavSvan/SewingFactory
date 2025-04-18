using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels.Base;

public class ProcessBasedEmployeeViewModel : EmployeeViewModel
{
    [Range(0, 100, ErrorMessage = "Premium must be between 0 and 100.")] public decimal Premium { get; set; } = 0;
}