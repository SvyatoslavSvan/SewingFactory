namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities;

public class GarmentCategory(string name, List<GarmentModel> garmentModels) : NamedIdentity(name)
{
    public IReadOnlyList<GarmentModel> GarmentModels => garmentModels;
}