using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.RateItems;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.ValueObjects;
using System.Reflection;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Tests;

public class CalculateSalaryTests
{
    [Fact]
    public void ProcessBasedEmployee_ShouldSetPaymentAndPremium()
    {
        // Arrange
        var dept = new Department("Cutting");
        var category = new GarmentCategory("Shirts");
        var processPrice = new Money(100m);
        var process = new Process("Sew", dept, processPrice);
        var model = new GarmentModel("M1", "desc", [process], category);
        var documentDate = new DateOnly(2025, 5, 1);
        var document = WorkshopDocument.CreateInstance("Doc", 1, documentDate, model, dept);

        var employee = new ProcessBasedEmployee("Ivan", "EMP001", dept, new Percent(10));
        var repeat = new EmployeeTaskRepeat(employee, 2);

        document.Tasks.First().AddEmployeeRepeat(repeat);
        document.RecalculateEmployees();
        ((ICollection<WorkshopDocument>)employee.Documents).Add(document);

        var period = new DateRange(new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 31));

        // Act
        var salary = employee.CalculateSalary(period);

        // Assert (2 × 100 = 200 ; premium 10 %)
        Assert.Equal(200m, salary.Payment.Amount);
        Assert.Equal(20m, salary.Premium.Amount);
        Assert.Equal(0m, salary.AdditionalPayment.Amount);
        Assert.Same(employee, salary.Employee);
        Assert.Equal(220m, salary.TakeHome.Amount);
    }

    [Fact]
    public void RateBasedEmployee_ShouldSetPaymentPremiumAndEmployee()
    {
        // Arrange
        var dept = new Department("Dept");
        var employee = new RateBasedEmployee("E", "EMP002", new Money(1000m), dept, 10);
        var ts = Timesheet.CreateInstance(new List<RateBasedEmployee> { employee },
            new DateOnly(2025, 5, 1));

        ((ICollection<Timesheet>)employee.Timesheets).Add(ts);

        var period = new DateRange(new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 31));

        // Act
        var salary = employee.CalculateSalary(period);

        // Assert (rate 1000, premium 10 %)
        Assert.Equal(1000m, salary.Payment.Amount);
        Assert.Equal(100m, salary.Premium.Amount);
        Assert.Equal(0m, salary.AdditionalPayment.Amount);
        Assert.Same(employee, salary.Employee);
        Assert.Equal(1100m, salary.TakeHome.Amount);
    }

    [Fact]
    public void Technologist_ShouldMoveBaseToAdditionalAndSetPercentPayment()
    {
        // ---------- Arrange ----------
        var department = new Department("Cutting");
        var category = new GarmentCategory("Shirts");
        var processPrice = new Money(100m);
        var process = new Process("Sew", department, processPrice);
        var garmentModel = new GarmentModel(
            "Model1",
            "desc",
            [process],
            category,
            null);

        var docDate = new DateOnly(2025, 5, 1);
        var document = WorkshopDocument.CreateInstance(
            "Doc1",
            1,
            docDate,
            garmentModel,
            department);

        var employee = new ProcessBasedEmployee("Ivan", "EMP001", department, new Percent(0));
        department.AddEmployee(employee);
        var technologist = new Technologist("tech", "EMP002", new Percent(10), department);
        var repeat = new EmployeeTaskRepeat(employee, 2);
        var technologistRepeat = new EmployeeTaskRepeat(technologist, 2);
        SetIds(employee, technologist);
        document.Tasks.First().AddEmployeeRepeat(repeat);
        document.Tasks.First().AddEmployeeRepeat(technologistRepeat);
        document.RecalculateEmployees();

        var docsCollection = employee.Documents as ICollection<WorkshopDocument>;
        docsCollection!.Add(document);

        var technologistDocsCollection = technologist.Documents as ICollection<WorkshopDocument>;
        technologistDocsCollection!.Add(document);

        // ---------- Act ----------
        var range = new DateRange(new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 31));
        var salary = technologist.CalculateSalary(range);

        // ---------- Assert ----------
        //200 * 10% = 20
        Assert.Equal(20m, salary.Payment.Amount);
        Assert.Equal(200m, salary.AdditionalPayment.Amount);
        Assert.Equal(0m, salary.Premium.Amount);
        Assert.Equal(20m + 200m, salary.TakeHome.Amount);
    }

    /* ---------- helpers ---------- */

    private static void SetIds(params Employee[] employees)
    {
        var idField = typeof(Identity)
            .GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!;

        foreach (var e in employees)
        {
            idField.SetValue(e, Guid.NewGuid());
        }
    }
}