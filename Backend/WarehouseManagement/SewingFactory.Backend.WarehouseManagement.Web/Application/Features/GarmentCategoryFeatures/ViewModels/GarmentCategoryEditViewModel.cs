using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;

public class GarmentCategoryEditViewModel : GarmentCategoryCreateViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}
