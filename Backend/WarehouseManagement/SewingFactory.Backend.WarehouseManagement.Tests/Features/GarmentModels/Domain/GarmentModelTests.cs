using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;

namespace SewingFactory.Backend.WarehouseManagement.Tests.Features.GarmentModels.Domain;

/// <summary>
///     Unit tests covering <see cref="GarmentModel" /> setters.
/// </summary>
public sealed class GarmentModelTests
{
    [Fact(DisplayName = "Category setter rejects null")]
    public void Category_Null_Throws()
    {
        var category = new WarehouseManagement.Domain.Entities.Garment.GarmentCategory("Outerwear", []);
        var model = new WarehouseManagement.Domain.Entities.Garment.GarmentModel("Jacket", category, null!);

        Assert.Throws<ArgumentNullException>(testCode: () => model.Category = null!);
    }
}
