using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public sealed class GarmentCategory : NamedIdentity
{
    private readonly List<GarmentModel> _products;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private GarmentCategory() => _products = [];

    public GarmentCategory(string name, List<GarmentModel> products) : base(name) => _products = products;

    public IReadOnlyList<GarmentModel> Products => _products;
}
