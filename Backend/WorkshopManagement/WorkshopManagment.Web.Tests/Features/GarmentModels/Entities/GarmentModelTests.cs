using SewingFactory.Backend.WorkshopManagement.Domain.Entities.DocumentItems;
using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.GarmentModels.Entities;

public sealed class GarmentModelTests
{
    private readonly string _validName = "Test Garment";
    private readonly string _validDescription = "Test Description";
    private readonly List<Process> _validProcesses = new();
    private readonly GarmentCategory _category = new("Test Category");
    private readonly byte[] _validImage = [1, 2, 3];
    private readonly Money _price = new Money(1000m);

    // Create an actual GarmentCategory instance
    // Initialize with empty processes list

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Act
        var garmentModel = new GarmentModel(
            _validName,
            _validDescription,
            _validProcesses,
            _category,
            _price,
            _validImage);

        // Assert
        Assert.Equal(_validName, garmentModel.Name);
        Assert.Equal(_validDescription, garmentModel.Description);
        Assert.Same(_validProcesses, garmentModel.Processes);
        Assert.Same(_category, garmentModel.Category);
        Assert.Same(_validImage, garmentModel.Image);
    }

    [Fact]
    public void Constructor_WithNullImage_CreatesInstanceWithNullImage()
    {
        // Act
        var garmentModel = new GarmentModel(
            _validName,
            _validDescription,
            _validProcesses,
            _category, _price);

        // Assert
        Assert.Null(garmentModel.Image);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Description_SetNullOrEmpty_ThrowsSewingFactoryArgumentNullException(string? invalidDescription)
    {
        // Arrange
        var garmentModel = new GarmentModel(
            _validName,
            _validDescription,
            _validProcesses,
            _category, _price);

        // Act & Assert
        var exception = Assert.Throws<SewingFactoryArgumentNullException>(testCode: () =>
            garmentModel.Description = invalidDescription!);

        Assert.Equal("The Description is NULL", exception.ParamName);
    }

    [Fact]
    public void Category_SetNull_ThrowsSewingFactoryArgumentNullException()
    {
        // Arrange
        var garmentModel = new GarmentModel(
            _validName,
            _validDescription,
            _validProcesses,
            _category, _price);

        // Act & Assert
        var exception = Assert.Throws<SewingFactoryArgumentNullException>(testCode: () =>
            garmentModel.Category = null!);

        Assert.Equal("The Category is NULL", exception.ParamName);
    }

    [Fact]
    public void ReplaceProcesses_WithEmptyList_ClearsProcesses()
    {
        // Arrange
        var garmentModel = new GarmentModel(
            _validName,
            _validDescription,
            new List<Process>(),
            _category, _price);

        // Act
        garmentModel.ReplaceProcesses([]);

        // Assert
        Assert.Empty(garmentModel.Processes);
    }
    
    [Fact]
    public void Clone_ReturnsNewInstance()
    {
        // Arrange
        var garmentModel = new GarmentModel(
            _validName,
            _validDescription,
            _validProcesses,
            _category, _price);

        // Act
        var clone = garmentModel.Clone();

        // Assert
        Assert.NotSame(garmentModel, clone);
    }
}