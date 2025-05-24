using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentCategoryFeatures.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;

public class GarmentModelReadViewModel : GarmentModelViewModel, IIdentityViewModel
{
    public required GarmentCategoryReadViewModel Category { get; set; }
    public Guid Id { get; set; }
}
