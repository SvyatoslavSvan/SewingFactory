using SewingFactory.Backend.WarehouseManagement.Domain.Entities;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Tests;

/// <summary>
///     Unit tests covering <see cref="GarmentModel" /> setters.
/// </summary>
public sealed class GarmentModelTests
{
    [Fact(DisplayName = "Category setter rejects null")]
    public void Category_Null_Throws()
    {
        var category = new GarmentCategory("Outerwear", []);
        var model = new GarmentModel("Jacket", category, default);

        Assert.Throws<ArgumentNullException>(testCode: () => model.Category = null!);
    }
}