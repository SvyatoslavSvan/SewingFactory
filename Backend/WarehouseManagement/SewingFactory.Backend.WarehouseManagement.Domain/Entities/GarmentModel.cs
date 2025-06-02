using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public sealed class GarmentModel : NamedIdentity
{
    private GarmentCategory _category = null!;

    /// <summary>
    ///     default constructor for EF Core
    /// </summary>
    private GarmentModel()
    {
    }

    public GarmentModel(string name, GarmentCategory category, Money price) : base(name)
    {
        Category = category;
        Price = price;
    }

    public GarmentCategory Category
    {
        get => _category;
        set => _category = value ?? throw new ArgumentNullException(nameof(Category));
    }

    public Money Price { get; set; } = null!;
}
