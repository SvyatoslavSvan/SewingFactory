using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.DepartmentMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels.Base;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ProcessEmployeeReadViewModel), "process")]
[JsonDerivedType(typeof(RateEmployeeReadViewModel), "rate")]
[JsonDerivedType(typeof(TechnologistReadViewModel), "technologist")]
public class EmployeeReadViewModel : EmployeeViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
    public required ReadDepartmentViewModel DepartmentViewModel { get; set; }
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