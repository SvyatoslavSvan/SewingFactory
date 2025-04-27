using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ProcessEmployeeReadViewModel), "process")]
[JsonDerivedType(typeof(RateEmployeeReadViewModel), "rate")]
[JsonDerivedType(typeof(TechnologistReadViewModel), "technologist")]
public class EmployeeReadViewModel
{
    public EmployeeReadViewModel()
    {
        
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string InternalId { get; set; } = default!;
    public Department Department { get; set; }
}

internal class ProcessEmployeeReadViewModel : EmployeeReadViewModel
{
    public decimal Premium { get; set; }
}

internal class RateEmployeeReadViewModel : EmployeeReadViewModel
{
    public decimal Rate { get; set; }
    public int PremiumPercent { get; set; }
}

internal class TechnologistReadViewModel : EmployeeReadViewModel
{
    public int SalaryPercentage { get; set; }
}