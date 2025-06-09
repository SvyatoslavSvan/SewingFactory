using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;

public sealed class InternalTransferOperation : Operation
{
    private readonly PointOfSale _receiver = null!;

    /// <summary>
    ///     default constructor for EF Core
    /// </summary>
    private InternalTransferOperation()
    {
    }

    public InternalTransferOperation(
        PointOfSale owner,
        int quantity,
        DateOnly date,
        PointOfSale receiver, StockItem stockItem) : base(owner,
        quantity,
        date, stockItem)
    {
        Receiver = receiver;
        ReceiverId = receiver.Id;
    }

    public PointOfSale Receiver
    {
        get => _receiver;
        init => _receiver = value ?? throw new SewingFactoryArgumentNullException(nameof(Receiver));
    }

    public Guid ReceiverId { get; private set; }
    public override string DisplayName => "Переміщення";
}
