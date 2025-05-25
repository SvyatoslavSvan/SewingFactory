using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.ViewModels;
using SewingFactory.Backend.WarehouseManagement.Web.Endpoints.Base;

namespace SewingFactory.Backend.WarehouseManagement.Web.Endpoints
{
    public class PointOfSaleRouter
        : CommandRouter<PointOfSale,
            PointOfSaleReadViewModel,
            PointOfSaleCreateViewModel,
            PointOfSaleEditViewModel,
            PointOfSaleDeleteViewModel,
            PointOfSaleReadViewModel>
    {
    }
}
