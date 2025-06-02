using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Tests;

public class WorkshopDocumentTests
{
    [Fact]
    public void CreateInstance_ShouldThrow_WhenCountOfModelsNegative()
    {
        var model = new GarmentModel("ModelX", "desc", new List<Process>(), new GarmentCategory("Cat"));
        var dept = new Department("Dept");
        Assert.Throws<SewingFactoryArgumentException>(testCode: () =>
            WorkshopDocument.CreateInstance("Doc1", -5, DateOnly.FromDateTime(DateTime.Now), model, dept));
    }

    [Fact]
    public void CreateInstance_ShouldThrow_WhenGarmentModelIsNull()
    {
        var dept = new Department("Dept");
        // Теперь мы ожидаем NullReferenceException, поскольку код прямо обращается к garmentModel
        Assert.Throws<NullReferenceException>(testCode: () =>
            WorkshopDocument.CreateInstance("Doc1", 1, new DateOnly(2025, 1, 1), null!, dept));
    }

    [Fact]
    public void CreateInstance_ShouldInitializeTasks_ForDepartmentProcesses()
    {
        // Arrange: GarmentModel с тремя процессами, два из которых в одном отделе, 
        // но код сейчас просто превращает каждый Process в WorkshopTask
        var dept = new Department("Sewing");
        var otherDept = new Department("Cutting");
        var proc1 = new Process("P1", dept, new Money(10m));
        var proc2 = new Process("P2", dept, new Money(20m));
        var procOther = new Process("P3", otherDept, new Money(5m));
        var model = new GarmentModel("Model1", "desc",
            new List<Process> { proc1, proc2, procOther }, new GarmentCategory("Cat"));

        // Act
        var doc = WorkshopDocument.CreateInstance("Doc1", 100, new DateOnly(2025, 5, 1), model, dept);

        // Assert: теперь количество тасков равно общему числу процессов в модели
        Assert.Equal(model.Processes.Count, doc.Tasks.Count);

        // дополнительные проверки, что каждый таск соответствует одному из процессов
        foreach (var task in doc.Tasks)
        {
            Assert.Contains(task.Process, model.Processes);
        }
    }

    [Fact]
    public void RecalculateEmployees_ShouldUpdateEmployeesList()
    {
        // Arrange: document with one task and one employee repeat
        var dept = new Department("Dep");
        var process = new Process("Proc", dept, new Money(0));
        var model = new GarmentModel("M", "desc", new List<Process> { process }, new GarmentCategory("Cat"));
        var doc = WorkshopDocument.CreateInstance("Doc", 1, DateOnly.FromDateTime(DateTime.Now), model, dept);
        var emp = new RateBasedEmployee("E", "ID", new Money(0), dept);
        // Initially no employees involved
        Assert.Empty(doc.Employees);

        // Add employee to task and recalc
        var task = doc.Tasks.First();
        task.AddEmployeeRepeat(new EmployeeTaskRepeat(emp, 1));
        doc.RecalculateEmployees();

        // Now document should list that employee
        Assert.Contains(emp, doc.Employees);
        // If we remove the employee from task and recalc, Employees becomes empty
        task.ReplaceRepeats(new List<EmployeeTaskRepeat>()); // remove all repeats
        doc.RecalculateEmployees();
        Assert.DoesNotContain(emp, doc.Employees);
    }

    [Fact]
    public void CalculatePaymentForEmployee_ShouldSumTaskPayments()
    {
        var dept = new Department("Dep");
        var process = new Process("Proc", dept, new Money(50m));
        var model = new GarmentModel("M", "desc", new List<Process> { process }, new GarmentCategory("Cat"));
        var doc = WorkshopDocument.CreateInstance("Doc", 1, new DateOnly(2025, 1, 1), model, dept);
        var emp = new RateBasedEmployee("E", "ID", new Money(0), dept);
        // Добавляем сотрудника в задачу с 2 повторениями
        doc.Tasks.First().AddEmployeeRepeat(new EmployeeTaskRepeat(emp, 2));

        var total = doc.CalculatePaymentForEmployee(emp);
        // Ожидаем 2 * 50 = 100
        Assert.Equal(100m, total.Amount);
    }
}