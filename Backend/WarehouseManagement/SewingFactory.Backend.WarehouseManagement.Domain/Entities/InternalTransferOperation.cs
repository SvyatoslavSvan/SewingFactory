using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public sealed class InternalTransferOperation : Operation
{
    private PointOfSale _receiver = null!;

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
        PointOfSale receiver) : base(owner,
        quantity,
        date)
    {
        Receiver = receiver;
        ReceiverId = receiver.Id;
    }

    public PointOfSale Receiver
    {
        get => _receiver;
        set => _receiver = value ?? throw new SewingFactoryArgumentNullException(nameof(Receiver));
    }

    public Guid ReceiverId { get; private set; }
}
