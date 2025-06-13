using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Departments.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels.Base;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.Employees.ViewModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ProcessEmployeeReadViewModel), "process")]
[JsonDerivedType(typeof(RateEmployeeReadViewModel), "rate")]
[JsonDerivedType(typeof(TechnologistReadViewModel), "technologist")]
public class EmployeeReadViewModel : EmployeeViewModel, IIdentityViewModel
{
    public required ReadDepartmentViewModel DepartmentViewModel { get; set; }
    public Guid Id { get; set; }
}

public class ProcessEmployeeReadViewModel : EmployeeReadViewModel
{
    public decimal Premium { get; set; }
}

public sealed class RateEmployeeReadViewModel : ProcessEmployeeReadViewModel
{
    public decimal Rate { get; set; }
}

public sealed class TechnologistReadViewModel : EmployeeReadViewModel
{
    public int SalaryPercentage { get; set; }
}