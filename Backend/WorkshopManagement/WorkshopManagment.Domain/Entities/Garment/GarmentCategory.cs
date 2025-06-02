namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;

public sealed class GarmentCategory : NamedIdentity
{
    private readonly List<GarmentModel> _garmentModels = null!;

    /// <summary>
    ///     Default constructor for EF Core
    /// </summary>
    private GarmentCategory()
    {
    }

    public GarmentCategory(string name) : base(name)
    {
    }

    public GarmentCategory(string name, List<GarmentModel> garmentModels) : base(name) => _garmentModels = garmentModels;

    public IReadOnlyList<GarmentModel> GarmentModels => _garmentModels;
}