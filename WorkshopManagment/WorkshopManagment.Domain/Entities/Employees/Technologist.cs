using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;

public class Technologist : ProcessBasedEmployee
{
    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private Technologist() => SalaryPercentage = 0;

    [SetsRequiredMembers]
    public Technologist(string name, string internalId, Percent salaryPercentage, Department department) : base(name, internalId, department, new Percent(0)) => SalaryPercentage = salaryPercentage;

    public Percent SalaryPercentage { get; set; }

    public override Salary CalculateSalary(
        DateRange dateRange)
        => new(new Money(SalaryPercentage.Value *
                         Department.Employees.Sum(selector: e => e.CalculateSalary(dateRange)
                             .Payment.Amount)),
            new Money(0),
            this,
            base.CalculateSalary(dateRange)
                .Payment);
}