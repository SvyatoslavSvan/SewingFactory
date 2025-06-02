using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Endpoints;

public class GarmentModelRouter : CommandRouter<GarmentModel,
    GarmentModelReadViewModel,
    GarmentModelCreateViewModel,
    GarmentModelEditViewModel,
    GarmentModelDeleteViewModel,
    GarmentModelReadViewModel>
{
}
