using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.ValueObjects;
using System.Reflection;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Tests;

public class SalaryPercentagePaymentTests
{
    [Fact]
    public void SalaryPercentagePayment_ShouldReturnPercentOfSalaries_OfEmployeesSubordinate()
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

        document.Tasks.First().AddEmployeeRepeat(repeat);
        document.RecalculateEmployees();

        var docsCollection = employee.Documents as ICollection<WorkshopDocument>;
        docsCollection!.Add(document);

        // ---------- Act ----------
        var range = new DateRange(new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 31));
        SetIdsForEmployees(employee, technologist);
        var payment = ((IHasSalaryPercentage)technologist).SalaryPercentagePayment(range);

        // ---------- Assert ----------
        //200 * 10% = 20
        Assert.Equal(20m, payment.Amount);
    }

    private static void SetIdsForEmployees(ProcessBasedEmployee employee, Technologist technologist)
    {
        typeof(Identity)
            .GetField("<Id>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(employee, Guid.NewGuid());

        typeof(Identity)
            .GetField("<Id>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(technologist, Guid.NewGuid());
    }
}