using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Features.Common.Base.Routers;
using SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.ViewModels;
using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Features.GarmentCategories.Routers;

public class GarmentCategoryRouter : CommandRouter<GarmentCategory, ReadGarmentCategoryViewModel, CreateGarmentCategoryViewModel, UpdateGarmentCategoryViewModel, DeleteGarmentCategoryViewModel,
    ReadGarmentCategoryViewModel>
{
    protected override string? PolicyName => AppData.DesignerAccess;
}