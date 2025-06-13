using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Features.PointOfSales.Provides.Interfaces;

public interface IAllOperationForStockReportProvider
{
    public byte[] Build(PointOfSale pointOfSale,GarmentModel garmentModel, DateRange period);    
}
