using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Tests;

public class EmployeeTaskRepeatTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenEmployeeIsNull()
    {
        // Repeats = 1, Employee = null
        Assert.Throws<SewingFactoryArgumentNullException>(() => new EmployeeTaskRepeat(null!, 1));
    }

    [Fact]
    public void Repeats_SetNegative_ShouldThrow()
    {
        var emp = new RateBasedEmployee("Name", "IntID", new Money(500m), new Department("Dept"));
        var repeat = new EmployeeTaskRepeat(emp, 2);
        Assert.Throws<SewingFactoryArgumentException>(() => repeat.Repeats = -5);
    }

    [Fact]
    public void WorkshopTask_SetNull_ShouldThrow()
    {
        var emp = new RateBasedEmployee("N", "ID", new Money(100m), new Department("D"));
        var repeat = new EmployeeTaskRepeat(emp, 1);
        Assert.Throws<SewingFactoryArgumentNullException>(() => repeat.WorkshopTask = null!);
    }
}