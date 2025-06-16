using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Processes.Entity;

public sealed class ProcessTests
{
    [Fact]
    public void Ctor_WhenPriceIsNull_DefaultsToZeroMoney()
    {
        // Arrange
        var dept = new Department("Cutting");

        // Act
        var process = new Process("Cutting Stitch", dept, price: null);

        // Assert
        Assert.Equal(0m, process.Price.Amount);
    }

    [Fact]
    public void Ctor_WhenPriceIsProvided_SetsThatPrice()
    {
        // Arrange
        var dept = new Department("Assembly");
        var expectedPrice = new Money(123.45m);

        // Act
        var process = new Process("Final Assembly", dept, expectedPrice);

        // Assert
        Assert.Equal(expectedPrice, process.Price);
    }

    [Fact]
    public void Ctor_SetsNameAndDepartment()
    {
        // Arrange
        var dept = new Department("Packaging");
        var name = "Box Packing";

        // Act
        var process = new Process(name, dept, new Money(10m));

        // Assert
        Assert.Equal(name, process.Name);
        Assert.Same(dept, process.Department);
    }

    [Fact]
    public void Price_Property_IsReadWrite()
    {
        // Arrange
        var dept = new Department("Labeling");
        var process = new Process("Label Application", dept, new Money(5m));
        var newPrice = new Money(42m);

        // Act
        process.Price = newPrice;

        // Assert
        Assert.Equal(newPrice, process.Price);
    }

    [Fact]
    public void Department_Property_IsReadWrite()
    {
        // Arrange
        var dept1 = new Department("Cutting");
        var dept2 = new Department("Sewing");
        var process = new Process("Cutting Stitch", dept1, new Money(2m));

        // Act
        process.Department = dept2;

        // Assert
        Assert.Same(dept2, process.Department);
    }
}