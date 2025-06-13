using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentCategories.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;

public class GarmentModelReadViewModel : GarmentModelViewModel, IIdentityViewModel
{
    public required GarmentCategoryReadViewModel Category { get; set; }
    public Guid Id { get; set; }
}
