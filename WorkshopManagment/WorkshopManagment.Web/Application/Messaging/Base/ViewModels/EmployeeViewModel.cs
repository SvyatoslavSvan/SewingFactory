using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;

public abstract class EmployeeViewModel
{
    [Required(ErrorMessage = "The 'Name' field is required.")] public string Name { get; set; } = default!;

    [Required(ErrorMessage = "The 'InternalId' field is required.")] public string InternalId { get; set; } = default!;

    [Required(ErrorMessage = "The 'Department' field is required.")] public Department Department { get; set; }
}