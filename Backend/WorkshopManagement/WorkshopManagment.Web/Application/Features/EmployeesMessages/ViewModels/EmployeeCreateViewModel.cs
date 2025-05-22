using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels.Base;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ProcessEmployeeCreateViewModel), "process")]
[JsonDerivedType(typeof(RateEmployeeCreateViewModel), "rate")]
[JsonDerivedType(typeof(TechnologistCreateViewModel), "technologist")]
public class EmployeeCreateViewModel : EmployeeViewModel
{
    public Guid DepartmentId { get; set; }
}

public class ProcessEmployeeCreateViewModel : EmployeeCreateViewModel
{
    public decimal Premium { get; set; }
}

public sealed class RateEmployeeCreateViewModel : ProcessEmployeeCreateViewModel
{
    public decimal Rate { get; set; }
}

public sealed class TechnologistCreateViewModel : EmployeeCreateViewModel
{
    public int SalaryPercentage { get; set; }
}