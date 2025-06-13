using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;

public sealed class PointOfSaleEditViewModel : PointOfSaleViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}
