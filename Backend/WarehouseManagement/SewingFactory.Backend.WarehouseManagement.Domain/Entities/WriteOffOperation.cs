using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public sealed class WriteOffOperation : Operation
{
    private WriteOffOperation()
    {
    }

    public WriteOffOperation(PointOfSale owner, int quantity, DateOnly date, StockItem stockItem)
        : base(owner, quantity, date, stockItem)
    {
    }

    public override string DisplayName => "Списання";
}
