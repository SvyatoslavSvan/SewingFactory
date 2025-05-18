using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;

public sealed class RateBasedEmployee : Employee, IHasRate, IHasPremium
{
    private readonly List<Timesheet> _timesheets = [];

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private RateBasedEmployee()
    {
        Rate = Money.Zero;
        Premium = new Percent(0);
    }

    [SetsRequiredMembers]
    public RateBasedEmployee(string name, string internalId, Money rate, Department department, int premium = 0) : base(name, internalId, department)
    {
        Rate = rate;
        Premium = premium;
    }

    public Money Rate { get; set; }

    public Percent Premium { get; set; }

    public IReadOnlyList<Timesheet> Timesheets => _timesheets;

    public override Salary CalculateSalary(DateRange dateRange)
    {
        var baseSalary = base.CalculateSalary(dateRange);
        baseSalary.Payment += ((IHasRate)this).RatePayment(dateRange);
        baseSalary.Premium = ((IHasPremium)this).PremiumPayment(baseSalary.Payment);
        return baseSalary;
    }

}