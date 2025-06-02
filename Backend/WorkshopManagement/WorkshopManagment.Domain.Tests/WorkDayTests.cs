using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Tests;

public class WorkDayTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(9)]
    public void Hours_SetInvalidValue_ShouldThrow(int invalidHours)
    {
        var employee = new RateBasedEmployee("Ivan", "ID123", new Money(1000m), new Department("Dept"));
        var workDay = new WorkDay(0, employee, new DateOnly(2025, 1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(testCode: () => workDay.Hours = invalidHours);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenEmployeeIsNull() =>
        // Act & Assert
        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () =>
            new WorkDay(8, null!, new DateOnly(2025, 1, 1)));

    [Fact]
    public void Hours_SetWithinRange_ShouldUpdateValue()
    {
        var employee = new RateBasedEmployee("Petr", "ID124", new Money(1000m), new Department("Dept"));
        var workDay = new WorkDay(8, employee, new DateOnly(2025, 1, 1));
        workDay.Hours = 5;
        Assert.Equal(5, workDay.Hours);
    }
}