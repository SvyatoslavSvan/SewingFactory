using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;

/// <summary>
///     Represents a workday for an employee.
/// </summary>
public sealed class WorkDay : Identity
{
    public const int MinimalWorkHoursValue = 0;
    public const int FullTimeWorkHoursValue = 8;
    private readonly RateBasedEmployee _employee = null!;

    private int _hours;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private WorkDay()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WorkDay" /> class.
    /// </summary>
    /// <param name="hours">The number of hours worked.</param>
    /// <param name="employee">The employee associated with the workday.</param>
    /// <param name="date">The date of the workday.</param>
    public WorkDay(int hours, RateBasedEmployee employee, DateOnly date)
    {
        Hours = hours;
        Employee = employee;
        Date = date;
    }

    // Properties
    /// <summary>
    ///     Gets or sets the number of hours worked.
    ///     Must be between <see cref="MinimalWorkHoursValue" /> and <see cref="FullTimeWorkHoursValue" />.
    /// </summary>
    public int Hours
    {
        get => _hours;
        set
        {
            if (value is < MinimalWorkHoursValue or > FullTimeWorkHoursValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"Value must be between {MinimalWorkHoursValue} and {FullTimeWorkHoursValue}."
                );
            }

            _hours = value;
        }
    }

    /// <summary>
    ///     Gets the employee associated with the workday.
    /// </summary>
    public RateBasedEmployee Employee
    {
        get => _employee;
        init => _employee = value ?? throw new SewingFactoryArgumentNullException(nameof(value));
    }

    /// <summary>
    ///     Gets the date of the workday.
    /// </summary>
    public DateOnly Date { get; init; }
}