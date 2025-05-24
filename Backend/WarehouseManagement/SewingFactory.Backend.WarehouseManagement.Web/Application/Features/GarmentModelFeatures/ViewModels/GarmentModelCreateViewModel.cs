using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.GarmentModelFeatures.ViewModels;

public class GarmentModelCreateViewModel : GarmentModelViewModel
{
    public Guid CategoryId { get; set; }
}
