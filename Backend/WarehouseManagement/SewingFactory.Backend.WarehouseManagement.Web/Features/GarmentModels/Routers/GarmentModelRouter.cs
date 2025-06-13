using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.Routers;

public class GarmentModelRouter : CommandRouter<GarmentModel,
    GarmentModelReadViewModel,
    GarmentModelCreateViewModel,
    GarmentModelEditViewModel,
    GarmentModelDeleteViewModel,
    GarmentModelReadViewModel>
{
}
