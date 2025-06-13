using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;

public class GarmentCategoryEditViewModel : GarmentCategoryCreateViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}
