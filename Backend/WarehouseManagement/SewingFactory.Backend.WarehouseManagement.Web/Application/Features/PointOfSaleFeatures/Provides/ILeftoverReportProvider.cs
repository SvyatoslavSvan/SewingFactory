using SewingFactory.Backend.WarehouseManagement.Domain.Entities;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides;

public interface ILeftoverReportProvider
{
    public byte[] Build(PointOfSale pointOfSale);
}
