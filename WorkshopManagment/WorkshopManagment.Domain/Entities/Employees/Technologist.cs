using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;

public class Technologist : Employee
{
    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private Technologist() => SalaryPercentage = 0;

    [SetsRequiredMembers]
    public Technologist(string name, string internalId, Percent salaryPercentage, Department department) : base(name, internalId, department) => SalaryPercentage = salaryPercentage;

    public Percent SalaryPercentage { get; set; }

    public override Salary CalculateSalary(DateRange dateRange) => throw new NotImplementedException();
}