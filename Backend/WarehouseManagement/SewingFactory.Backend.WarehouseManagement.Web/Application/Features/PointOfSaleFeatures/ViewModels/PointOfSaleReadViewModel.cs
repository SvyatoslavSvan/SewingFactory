using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.Base.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels
{
    public sealed class PointOfSaleReadViewModel : PointOfSaleViewModel, IIdentityViewModel
    {
        public Guid Id { get; set; }
    }
}
