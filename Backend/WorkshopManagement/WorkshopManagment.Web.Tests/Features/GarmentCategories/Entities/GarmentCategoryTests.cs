using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;

namespace SewingFactory.Backend.WorkshopManagement.Tests.Features.GarmentCategories.Entities;

public sealed class GarmentCategoryTests
{
    private readonly string _validName = "Test Category";
    
    [Fact]
    public void Constructor_WithNameOnly_CreatesInstance()
    {
        // Act
        var category = new GarmentCategory(_validName);
        
        // Assert
        Assert.Equal(_validName, category.Name);
    }
    
    [Fact]
    public void Constructor_WithNameAndModels_CreatesInstance()
    {
        // Arrange
        var garmentModels = new List<GarmentModel>();
        
        // Act
        var category = new GarmentCategory(_validName, garmentModels);
        
        // Assert
        Assert.Equal(_validName, category.Name);
        Assert.Same(garmentModels, category.GarmentModels);
        Assert.NotNull(category.GarmentModels);
    }
    
    [Fact]
    public void GarmentModels_ReturnsReadOnlyList_WhenInitializedWithList()
    {
        // Arrange
        var garmentModels = new List<GarmentModel>();
        var category = new GarmentCategory(_validName, garmentModels);
        
        // Act
        var models = category.GarmentModels;
        
        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<GarmentModel>>(models);
    }
}