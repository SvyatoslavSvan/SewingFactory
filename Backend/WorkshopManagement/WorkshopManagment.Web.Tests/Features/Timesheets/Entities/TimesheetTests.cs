using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Timesheets.Entities;

public sealed class TimesheetTests
{
    [Fact]
    public void CreateInstance_ShouldThrow_WhenEmployeeListEmpty()
    {
        var date = new DateOnly(2025, 1, 1);
        var employees = new List<RateBasedEmployee>();
        Assert.Throws<SewingFactoryArgumentException>(testCode: () => Timesheet.CreateInstance(employees, date));
    }

    [Fact]
    public void CreateInstance_ShouldGenerateWorkDays_ForAllEmployees()
    {
        var date = new DateOnly(2025, 1, 1);
        var emp = new RateBasedEmployee("E", "ID", new Money(0), new Department("D"));
        var timesheet = Timesheet.CreateInstance(new List<RateBasedEmployee> { emp }, date);
        Assert.Equal(23, timesheet.DaysCount);
        Assert.Equal(23 * WorkDay.FullTimeWorkHoursValue, timesheet.Hours);
        Assert.Equal(31, timesheet.WorkDays.Count);
        foreach (var wd in timesheet.WorkDays)
        {
            if (wd.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                Assert.Equal(WorkDay.MinimalWorkHoursValue, wd.Hours);
            }
            else
            {
                Assert.Equal(WorkDay.FullTimeWorkHoursValue, wd.Hours);
            }
        }
    }

    [Fact]
    public void HoursWorked_ShouldSumHours_ForSpecificEmployee()
    {
        var date = new DateOnly(2025, 5, 1);
        var emp1 = new RateBasedEmployee("A", "1", new Money(0), new Department("Dep"));
        var emp2 = new RateBasedEmployee("B", "2", new Money(0), new Department("Dep"));
        var timesheet = Timesheet.CreateInstance(new List<RateBasedEmployee> { emp1, emp2 }, date);
        Assert.Equal(timesheet.Hours, timesheet.HoursWorked(emp1));
        Assert.Equal(timesheet.Hours, timesheet.HoursWorked(emp2));
    }
}