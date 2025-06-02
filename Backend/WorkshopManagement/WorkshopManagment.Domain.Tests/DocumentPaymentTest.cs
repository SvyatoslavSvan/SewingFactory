using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Interfaces;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Tests;

public class DocumentPaymentTests
{
    [Fact]
    public void DocumentPayment_ShouldReturnSum_ForDocumentsInPeriod()
    {
        // ---------- Arrange ----------
        var dept = new Department("Cutting");
        var category = new GarmentCategory("Shirts");
        var processPrice = new Money(100m);
        var process = new Process("Sew", dept, processPrice);
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
            dept);

        var employee = new RateBasedEmployee("Ivan", "EMP001", new Money(0), dept);
        var repeat = new EmployeeTaskRepeat(employee, 2);

        document.Tasks.First().AddEmployeeRepeat(repeat);
        document.RecalculateEmployees();

        var docsCollection = employee.Documents as ICollection<WorkshopDocument>;
        docsCollection!.Add(document);

        // ---------- Act ----------
        var range = new DateRange(new DateOnly(2025, 5, 1), new DateOnly(2025, 5, 31));
        var payment = ((IHasDocuments)employee).DocumentPayment(range);

        // ---------- Assert ----------
        //2  * 100 = 200
        Assert.Equal(200m, payment.Amount);
    }
}