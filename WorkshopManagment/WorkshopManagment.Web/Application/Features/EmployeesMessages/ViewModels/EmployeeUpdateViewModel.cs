using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels.Base;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ProcessEmployeeUpdateViewModel), "process")]
[JsonDerivedType(typeof(RateEmployeeUpdateViewModel), "rate")]
[JsonDerivedType(typeof(TechnologistUpdateViewModel), "technologist")]
public class EmployeeUpdateViewModel : EmployeeViewModel, IIdentityViewModel
{
    public Guid DepartmentId { get; set; }
    public Guid Id { get; set; }
}

public class ProcessEmployeeUpdateViewModel : EmployeeUpdateViewModel
{
    public decimal Premium { get; set; }
}

public sealed class RateEmployeeUpdateViewModel : ProcessEmployeeUpdateViewModel
{
    public decimal Rate { get; set; }
}

public sealed class TechnologistUpdateViewModel : ProcessEmployeeUpdateViewModel
{
    public int SalaryPercentage { get; set; }
}