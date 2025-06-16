using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;
using System.Reflection;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.WorkshopDocuments.Entities;

public sealed class WorkshopDocumentTests
{
    [Fact]
    public void CreateInstance_ShouldThrow_WhenCountOfModelsNegative()
    {
        var model = new GarmentModel("ModelX", "desc", new List<Process>(), new GarmentCategory("Cat"), Money.Zero);
        var dept = new Department("Dept");
        Assert.Throws<SewingFactoryArgumentException>(testCode: () =>
            WorkshopDocument.CreateInstance("Doc1", -5, DateOnly.FromDateTime(DateTime.Now), model, dept));
    }

    [Fact]
    public void CreateInstance_ShouldThrow_WhenGarmentModelIsNull()
    {
        var dept = new Department("Dept");
        Assert.Throws<NullReferenceException>(testCode: () =>
            WorkshopDocument.CreateInstance("Doc1", 1, new DateOnly(2025, 1, 1), null!, dept));
    }

    [Fact]
    public void CreateInstance_ShouldInitializeTasks_ForDepartmentProcesses()
    {
        var dept = new Department("Sewing");
        var otherDept = new Department("Cutting");
        var proc1 = new Process("P1", dept, new Money(10m));
        var proc2 = new Process("P2", dept, new Money(20m));
        var procOther = new Process("P3", otherDept, new Money(5m));
        var model = new GarmentModel("Model1", "desc",
            new List<Process> { proc1, proc2, procOther }, new GarmentCategory("Cat"), Money.Zero);

        // Act
        var doc = WorkshopDocument.CreateInstance("Doc1", 100, new DateOnly(2025, 5, 1), model, dept);

        // Assert: 
        Assert.Equal(model.Processes.Count, doc.Tasks.Count);

        foreach (var task in doc.Tasks)
        {
            Assert.Contains(task.Process, model.Processes);
        }
    }

    [Fact]
    public void RecalculateEmployees_ShouldUpdateEmployeesList()
    {
        // Arrange
        var dept = new Department("Dep");
        var process = new Process("Proc", dept, new Money(0));
        var model = new GarmentModel("M", "desc", new List<Process> { process }, new GarmentCategory("Cat"), Money.Zero);
        var doc = WorkshopDocument.CreateInstance("Doc", 1, DateOnly.FromDateTime(DateTime.Now), model, dept);
        var emp = new RateBasedEmployee("E", "ID", new Money(0), dept);
        Assert.Empty(doc.Employees);

        var task = doc.Tasks.First();
        task.AddEmployeeRepeat(new EmployeeTaskRepeat(emp, 1));
        doc.RecalculateEmployees();

        Assert.Contains(emp, doc.Employees);
        task.ReplaceRepeats(new List<EmployeeTaskRepeat>());
        doc.RecalculateEmployees();
        Assert.DoesNotContain(emp, doc.Employees);
    }

    [Fact]
    public void CalculatePaymentForEmployee_ShouldSumTaskPayments()
    {
        var dept = new Department("Dep");
        var process = new Process("Proc", dept, new Money(50m));
        var model = new GarmentModel("M", "desc", new List<Process> { process }, new GarmentCategory("Cat"), Money.Zero);
        var doc = WorkshopDocument.CreateInstance("Doc", 1, new DateOnly(2025, 1, 1), model, dept);
        var emp = new RateBasedEmployee("E", "ID", new Money(0), dept);
        doc.Tasks.First().AddEmployeeRepeat(new EmployeeTaskRepeat(emp, 2));

        var total = doc.CalculatePaymentForEmployee(emp);
        Assert.Equal(100m, total.Amount);
    }

    [Fact]
    public void UpdateTaskRepeats_ShouldReplaceRepeatsAndRecalculateEmployees()
    {
        // Arrange
        var dept = new Department("Dep");
        var process = new Process("Proc", dept, new Money(5m));
        var model = new GarmentModel("M", "desc", [process], new GarmentCategory("Cat"), Money.Zero);
        var doc = WorkshopDocument.CreateInstance("Doc", 1, new DateOnly(2025, 6, 1), model, dept);
        var task = doc.Tasks.First();

        var emp1 = new RateBasedEmployee("Alice", "A1", new Money(0), dept);
        var emp2 = new RateBasedEmployee("Bob",   "B2", new Money(0), dept);

        var idField = typeof(Identity)
                .GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
            ;
        idField.SetValue(emp1, Guid.NewGuid());
        idField.SetValue(emp2, Guid.NewGuid());

        var initialRepeat = new EmployeeTaskRepeat(emp1, 2);
        idField.SetValue(initialRepeat, Guid.NewGuid());
        task.AddEmployeeRepeat(initialRepeat);

        doc.RecalculateEmployees();
        Assert.Contains(emp1, doc.Employees);

        var newRepeat = new EmployeeTaskRepeat(emp2, 3);

        // Act
        doc.UpdateTaskRepeats([
            new TaskRepeatInfo(task.Id, [newRepeat])
        ]);

        // Assert
        var repeats = task.EmployeeTaskRepeats;
        Assert.Single(repeats);
        Assert.Equal(emp2, repeats[0].WorkShopEmployee);
        Assert.Equal(3,    repeats[0].Repeats);

        Assert.DoesNotContain(emp1, doc.Employees);
        Assert.Contains(emp2,    doc.Employees);
    }

    [Fact]
    public void UpdateTaskRepeats_ShouldThrow_WhenTaskNotFound()
    {
        // Arrange
        var dept = new Department("Dep");
        var process = new Process("Proc", dept, new Money(5m));
        var model = new GarmentModel("M", "desc", new List<Process> { process }, new GarmentCategory("Cat"), Money.Zero);
        var doc = WorkshopDocument.CreateInstance("Doc", 1, new DateOnly(2025, 6, 1), model, dept);

        var bogusTaskId = Guid.NewGuid();
        var emp = new RateBasedEmployee("Alice", "A1", new Money(0), dept);
        var repeats = new List<EmployeeTaskRepeat> { new(emp, 1) };

        // Act & Assert
        Assert.Throws<SewingFactoryNotFoundException>(testCode: () =>
            doc.UpdateTaskRepeats([new TaskRepeatInfo(bogusTaskId, repeats)]));
    }
}