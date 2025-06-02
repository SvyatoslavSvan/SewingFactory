using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;

public sealed class PointOfSaleEditViewModel : PointOfSaleViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}
