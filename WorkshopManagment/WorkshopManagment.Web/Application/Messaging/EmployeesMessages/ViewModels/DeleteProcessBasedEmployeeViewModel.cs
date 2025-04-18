using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

public class DeleteProcessBasedEmployeeViewModel
{
    [Required(ErrorMessage = "Id is required")] public Guid Id { get; set; }
}