using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations.Base;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;

public sealed class SaleOperation : Operation
{
    private SaleOperation() { }

    public SaleOperation(PointOfSale owner, int quantity, DateOnly date, StockItem stockItem) : base(owner, quantity, date, stockItem)
    {
    }

    public override string DisplayName => "Продаж";
}
