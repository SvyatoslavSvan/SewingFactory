using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels.Base;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Features.EmployeesMessages.ViewModels;

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

public sealed class TechnologistReadViewModel : ProcessEmployeeReadViewModel
{
    public int SalaryPercentage { get; set; }
}