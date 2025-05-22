using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public class GarmentCategory(string name, List<GarmentModel> products) : NamedIdentity(name)
{
    public IReadOnlyList<GarmentModel> Products => products;
}
