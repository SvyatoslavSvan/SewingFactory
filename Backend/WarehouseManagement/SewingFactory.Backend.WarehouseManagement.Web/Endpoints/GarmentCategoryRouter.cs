using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Endpoints;

public class GarmentCategoryRouter
    : CommandRouter<GarmentCategory,
        GarmentCategoryReadViewModel,
        GarmentCategoryCreateViewModel,
        GarmentCategoryEditViewModel,
        GarmentCategoryDeleteViewModel,
        GarmentCategoryReadViewModel>
{
}
