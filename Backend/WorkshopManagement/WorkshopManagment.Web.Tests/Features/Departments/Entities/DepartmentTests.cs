using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Employees;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.Departments.Entities;

public sealed class DepartmentTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty() =>
        // Act & Assert
        Assert.Throws<SewingFactoryArgumentNullException>(testCode: () => new Department(string.Empty));

    [Fact]
    public void Constructor_ShouldSetName_WhenNameIsValid()
    {
        var dept = new Department("Cutting");
        Assert.Equal("Cutting", dept.Name);
    }
}