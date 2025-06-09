using SewingFactory.Backend.WarehouseManagement.Domain.Entities;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Web.Application.Features.PointOfSaleFeatures.Provides.Interfaces;

public interface IAllSalesForGarmentModelReportProvider
{
    public byte[] Build(IList<SaleOperation> sales, DateRange dateRange);   
}
