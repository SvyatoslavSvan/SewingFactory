using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Routers;

public class GarmentCategoryRouter : CommandRouter<GarmentCategory, ReadGarmentCategoryViewModel, CreateGarmentCategoryViewModel, UpdateGarmentCategoryViewModel, DeleteGarmentCategoryViewModel,
    ReadGarmentCategoryViewModel>
{
}