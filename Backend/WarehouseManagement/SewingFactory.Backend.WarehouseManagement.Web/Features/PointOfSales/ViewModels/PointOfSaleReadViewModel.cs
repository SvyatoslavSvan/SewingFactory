using SewingFactory.Backend.WarehouseManagement.Web.Features.Common.Base.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.ViewModels;

public class PointOfSaleReadViewModel : PointOfSaleViewModel, IIdentityViewModel
{
    public Guid Id { get; set; }
}
