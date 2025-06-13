using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Departments.Entities;

public sealed class DepartmentTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty() =>
        // Act & Assert
        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () => new Domain.Entities.Employees.Department(string.Empty));

    [Fact]
    public void Constructor_ShouldSetName_WhenNameIsValid()
    {
        var dept = new Domain.Entities.Employees.Department("Cutting");
        Assert.Equal("Cutting", dept.Name);
    }
}