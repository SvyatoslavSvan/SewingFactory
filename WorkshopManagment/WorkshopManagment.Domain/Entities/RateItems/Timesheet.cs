using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;

/// <summary>
///     Represents a timesheet containing workdays for employees over a specified month.
/// </summary>
public sealed class Timesheet : Identity
{
    private readonly List<WorkDay> _workDays = [];
    private readonly List<RateBasedEmployee> _employees = [];

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private Timesheet() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Timesheet" /> class.
    /// </summary>
    /// <param name="workDays">A list of workdays for the timesheet.</param>
    /// <param name="date">The date representing the month and year of the timesheet.</param>
    /// <param name="hours">The total number of working hours in the month.</param>
    /// <param name="daysCount">The total number of working days in the month.</param>
    /// <param name="employees">Employees involved into timesheet</param>
    private Timesheet(List<WorkDay> workDays, DateOnly date, int hours, int daysCount, IList<RateBasedEmployee> employees)
    {
        _workDays = workDays;
        Date = date;
        Hours = hours;
        DaysCount = daysCount;
        _employees = employees.ToList();
    }

    /// <summary>
    ///     Gets the total number of working hours in the timesheet.
    /// </summary>
    public int Hours { get; }

    /// <summary>
    ///     Gets the total number of working days in the timesheet.
    /// </summary>
    public int DaysCount { get; }

    /// <summary>
    ///     Gets the date representing the month and year of the timesheet.
    /// </summary>
    public DateOnly Date { get; }

    /// <summary>
    ///     Gets the workdays in the timesheet.
    /// </summary>
    public IReadOnlyList<WorkDay> WorkDays => _workDays;

    public IReadOnlyList<RateBasedEmployee> Employees => _employees;

    /// <summary>
    ///     Creates an instance of <see cref="Timesheet" /> for the specified employees, month, and year.
    /// </summary>
    /// <param name="employees">A list of employees for whom the timesheet is created.</param>
    /// <param name="date">The date representing the month and year of the timesheet.</param>
    /// <returns>A new instance of <see cref="Timesheet" />.</returns>
    /// <exception cref="SewingFactoryArgumentException">Thrown when the list of employees is empty.</exception>
    public static Timesheet CreateInstance(IList<RateBasedEmployee> employees, DateOnly date)
    {
        if (employees.Count == 0)
        {
            throw new SewingFactoryArgumentException(nameof(employees), "List of employees must not be empty.");
        }

        var daysCount = DateTime.DaysInMonth(date.Year, date.Month);
        var workDays = new List<WorkDay>(daysCount * employees.Count);

        foreach (var employee in employees)
        {
            for (var day = 1; day <= daysCount; day++)
            {
                var currentDate = new DateOnly(date.Year, date.Month, day);
                workDays.Add(IsWeekend(currentDate)
                    ? new WorkDay(WorkDay.MinimalWorkHoursValue, employee, currentDate)
                    : new WorkDay(WorkDay.FullTimeWorkHoursValue, employee, currentDate));
            }
        }

        var hoursDays = GetHoursDays(date);

        return new Timesheet(workDays, date, hoursDays.Item1, hoursDays.Item2,employees);
    }

    public int HoursWorked(IHasRate employee) => _workDays.Where(predicate: x => x.Employee == employee).Sum(selector: x => x.Hours);

    /// <summary>
    ///     Determines whether the specified date is a weekend.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><see langword="true" /> if the date is a Saturday or Sunday; otherwise, <see langword="false" />.</returns>
    private static bool IsWeekend(DateOnly date) =>
        date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    ///     Calculates the total number of working hours and working days in a given month.
    /// </summary>
    /// <param name="date">The date representing the month and year for which the calculation is performed.</param>
    /// <returns>
    ///     A tuple containing:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>The total number of working hours for all full-time workdays in the month.</description>
    ///         </item>
    ///         <item>
    ///             <description>The total number of full-time working days in the month.</description>
    ///         </item>
    ///     </list>
    /// </returns>
    /// <remarks>
    ///     This method assumes that weekends (Saturdays and Sundays) are non-working days.
    ///     The working hours for a full-time workday are defined by <see cref="WorkDay.FullTimeWorkHoursValue" />.
    /// </remarks>
    private static (int, int) GetHoursDays(DateOnly date)
    {
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var fullTimeWorkDaysCount = 0;
        for (var day = 1; day <= daysInMonth; day++)
        {
            var currentDate = new DateOnly(date.Year, date.Month, day);
            if (!IsWeekend(currentDate))
            {
                fullTimeWorkDaysCount++;
            }
        }

        return (fullTimeWorkDaysCount * WorkDay.FullTimeWorkHoursValue, fullTimeWorkDaysCount);
    }
}