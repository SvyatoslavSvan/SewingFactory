using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels.Base;

public class IdentityProcessBasedEmployeeViewModel : ProcessBasedEmployeeViewModel
{
    [Required(ErrorMessage = "Id is required")] public Guid Id { get; set; }
}