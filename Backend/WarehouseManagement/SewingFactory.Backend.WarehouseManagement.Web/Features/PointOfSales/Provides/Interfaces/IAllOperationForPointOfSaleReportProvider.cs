using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;

public interface IAllOperationForPointOfSaleReportProvider
{
    public byte[] Build(PointOfSale pointOfSale,DateRange period);  
}
