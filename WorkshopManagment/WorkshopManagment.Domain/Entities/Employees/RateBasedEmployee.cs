using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Enums;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;

public sealed class RateBasedEmployee : ProcessBasedEmployee
{
    private readonly List<Timesheet> _timesheets = [];

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private RateBasedEmployee() => Rate = new Money(0);

    [SetsRequiredMembers]
    public RateBasedEmployee(string name, string internalId, Money rate, Department department, int premium = 0) : base(name, internalId, department, premium) => Rate = rate;

    public Money Rate { get; set; }

    public IReadOnlyList<Timesheet> Timesheets => _timesheets;

    public override Salary CalculateSalary(DateRange dateRange)
    {
        var documentsPayment = base.CalculateSalary(dateRange).Payment;
        var timesheets = _timesheets.Where(predicate: x => dateRange.Contains(x.Date));
        var ratePayment = new Money(0);
        ratePayment = timesheets.Aggregate(ratePayment, func: (current, timesheet) => current + (timesheet.HoursWorked(this) * new Money(Rate.Amount / timesheet.Hours)));

        return new Salary(documentsPayment + ratePayment, (documentsPayment + ratePayment) * Premium, this);
    }
}