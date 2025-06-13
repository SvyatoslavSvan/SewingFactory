using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;

public class GarmentCategoryReadViewModel : GarmentCategoryViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}
