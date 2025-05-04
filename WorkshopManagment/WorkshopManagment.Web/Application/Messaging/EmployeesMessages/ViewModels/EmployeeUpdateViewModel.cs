using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Base.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels.Base;
using System.Text.Json.Serialization;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.EmployeesMessages.ViewModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ProcessEmployeeUpdateViewModel), "process")]
[JsonDerivedType(typeof(RateEmployeeUpdateViewModel), "rate")]
[JsonDerivedType(typeof(TechnologistUpdateViewModel), "technologist")]
public class EmployeeUpdateViewModel : EmployeeViewModel, IIdentityViewModel
{
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

public sealed class TechnologistUpdateViewModel : EmployeeUpdateViewModel
{
    public int SalaryPercentage { get; set; }
}