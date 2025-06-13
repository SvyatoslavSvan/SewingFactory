using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.Routers;

public class GarmentCategoryRouter
    : CommandRouter<GarmentCategory,
        GarmentCategoryReadViewModel,
        GarmentCategoryCreateViewModel,
        GarmentCategoryEditViewModel,
        GarmentCategoryDeleteViewModel,
        GarmentCategoryReadViewModel>
{
}
