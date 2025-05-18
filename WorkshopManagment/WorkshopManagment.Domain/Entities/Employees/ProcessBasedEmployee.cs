using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;

public class ProcessBasedEmployee : Employee, IHasPremium
{
    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    protected ProcessBasedEmployee() => Premium = 0;

    [SetsRequiredMembers]
    public ProcessBasedEmployee(string name, string internalId, Department department) : base(name, internalId, department) => Premium = new Percent(0);

    [SetsRequiredMembers]
    public ProcessBasedEmployee(string name, string internalId, Department department, Percent premium) : base(name, internalId, department) => Premium = premium;

    public Percent Premium { get; set; }

    public override Salary CalculateSalary(DateRange dateRange)
    {
        var baseSalary =  base.CalculateSalary(dateRange);
        baseSalary.Premium = ((IHasPremium)this).PremiumPayment(baseSalary.Payment);
        return baseSalary;
    }
}