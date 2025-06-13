using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.ViewModels;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;

public class GarmentModelEditViewModel : GarmentModelCreateViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}
