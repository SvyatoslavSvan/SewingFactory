using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;

public class GarmentModelEditViewModel : GarmentModelCreateViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}
