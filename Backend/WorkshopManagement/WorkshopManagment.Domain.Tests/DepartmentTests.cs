using SewingFactory.Backend.WorkshopManagement.Domain.Entities;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Tests;

public class DepartmentTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty()
    {
        // Act & Assert
        Assert.Throws<SewingFactoryArgumentNullException>(() => new Department(string.Empty));
    }

    [Fact]
    public void Constructor_ShouldSetName_WhenNameIsValid()
    {
        var dept = new Department("Cutting");
        Assert.Equal("Cutting", dept.Name);
    }
}