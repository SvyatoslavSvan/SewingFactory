using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public sealed class InternalTransferOperation : Operation
{
    private PointOfSale _receiver = null!;

    public InternalTransferOperation(PointOfSale pointOfSale,
        int quantity,
        DateOnly date,
        PointOfSale receiver) : base(pointOfSale,
        quantity,
        date,
        OperationType.InternalTransfer)
        => Receiver = receiver;

    public PointOfSale Receiver
    {
        get => _receiver;
        set => _receiver = value ?? throw new SewingFactoryArgumentNullException(nameof(Receiver));
    }
}