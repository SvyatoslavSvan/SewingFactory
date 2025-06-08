using SewingFactory.Backend.WarehouseManagement.Domain.Entities;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;

public interface ILeftoverReportProvider
{
    public byte[] Build(PointOfSale pointOfSale);
}
