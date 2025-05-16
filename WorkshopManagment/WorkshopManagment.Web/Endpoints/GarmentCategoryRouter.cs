using SewingFactory.Backend.WorkshopManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.GarmentCategoryMessages.ViewModels;
using SewingFactory.Backend.WorkshopManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WorkshopManagement.Web.Endpoints;

public class GarmentCategoryRouter : CommandRouter<GarmentCategory, ReadGarmentCategoryViewModel, CreateGarmentCategoryViewModel, UpdateGarmentCategoryViewModel, DeleteGarmentCategoryViewModel,
    ReadGarmentCategoryViewModel>
{
}