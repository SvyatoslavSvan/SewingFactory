using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;

public sealed class Technologist : Employee, IHasSalaryPercentage
{
    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private Technologist() => SalaryPercentage = 0;

    [SetsRequiredMembers]
    public Technologist(string name, string internalId, Percent salaryPercentage, Department department) : base(name, internalId, department) => SalaryPercentage = salaryPercentage;

    public Percent SalaryPercentage { get; set; }

    public override Salary CalculateSalary(DateRange dateRange)
    {
        var baseSalary = base.CalculateSalary(dateRange);
        baseSalary.AdditionalPayment = baseSalary.Payment;
        baseSalary.Payment = Money.Zero;
        baseSalary.Payment = ((IHasSalaryPercentage)this).SalaryPercentagePayment(dateRange);

        return baseSalary;
    }
}