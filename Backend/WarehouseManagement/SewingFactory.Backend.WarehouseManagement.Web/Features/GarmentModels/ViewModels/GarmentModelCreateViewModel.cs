using SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.GarmentModels.ViewModels;

public class GarmentModelCreateViewModel : GarmentModelViewModel
{
    public Guid CategoryId { get; set; }
}
