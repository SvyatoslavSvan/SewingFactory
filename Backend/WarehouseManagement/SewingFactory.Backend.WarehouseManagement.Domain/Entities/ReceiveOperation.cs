using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public sealed class ReceiveOperation : Operation
{
    private ReceiveOperation()
    {
    }

    public ReceiveOperation(PointOfSale owner, int quantity, DateOnly date, StockItem stockItem)
        : base(owner, quantity, date, stockItem)
    {
    }

    public override string DisplayName => "Відвантаження";
}
