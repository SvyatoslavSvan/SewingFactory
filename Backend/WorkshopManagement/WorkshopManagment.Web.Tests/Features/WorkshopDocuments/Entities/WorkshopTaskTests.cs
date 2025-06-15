using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.WorkshopDocuments.Entities;

public sealed class WorkshopTaskTests
{
    [Fact]
    public void AddEmployeeRepeat_ShouldAdd_WhenNewEmployee()
    {
        var process = new Process("Proc1", new Department("Dept"), new Money(100m));
        var task = new WorkshopTask(process);
        var emp = new RateBasedEmployee("E", "ID", new Money(0), process.Department);
        var repeat = new EmployeeTaskRepeat(emp, 3);

        task.AddEmployeeRepeat(repeat);
        Assert.Contains(emp, task.EmployeesInvolved);
        Assert.Equal(task, repeat.WorkshopTask);
    }

    [Fact]
    public void AddEmployeeRepeat_ShouldNotDuplicate_SameEmployee()
    {
        var process = new Process("Proc1", new Department("Dept"), new Money(50m));
        var task = new WorkshopTask(process);
        var emp = new RateBasedEmployee("E", "ID", new Money(0), process.Department);
        var repeat1 = new EmployeeTaskRepeat(emp, 1);
        var repeat2 = new EmployeeTaskRepeat(emp, 2);

        task.AddEmployeeRepeat(repeat1);
        task.AddEmployeeRepeat(repeat2);
        Assert.Single(task.EmployeeTaskRepeats);
        Assert.Equal(1, task.EmployeeTaskRepeats[0].Repeats);
    }

    [Fact]
    public void CalculatePaymentForEmployee_ShouldReturn_ProductOfRepeatsAndPrice()
    {
        var process = new Process("Proc1", new Department("Dept"), new Money(200m));
        var task = new WorkshopTask(process);
        var emp = new RateBasedEmployee("E", "ID", new Money(0), process.Department);
        task.AddEmployeeRepeat(new EmployeeTaskRepeat(emp, 4));

        var payment = task.CalculatePaymentForEmployee(emp);
        Assert.Equal(800m, payment.Amount);
    }

    [Fact]
    public void CalculatePaymentForEmployee_ShouldThrow_WhenEmployeeNotInTask()
    {
        var process = new Process("Proc1", new Department("Dept"), new Money(100m));
        var task = new WorkshopTask(process);
        var otherEmp = new RateBasedEmployee("X", "ID2", new Money(0), process.Department);
        Assert.Throws<SewingFactoryInvalidOperationException>(testCode: () => task.CalculatePaymentForEmployee(otherEmp));
    }
}