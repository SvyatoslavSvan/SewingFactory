using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;

public interface ILeftoverReportProvider
{
    public byte[] Build(PointOfSale pointOfSale);
}
