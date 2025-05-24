using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public class Operation : Identity
{
    private PointOfSale _owner = null!;
    private int _quantity;

    /// <summary>
    /// default constructor for EF Core
    /// </summary>
    protected Operation()
    {
        
    }

    public Operation(PointOfSale owner, int quantity, DateOnly date, OperationType operationType)
    {
        Owner = owner;
        Quantity = quantity;
        Date = date;
        OperationType = operationType;
        OwnerId = owner.Id;
    }

    public OperationType OperationType { get; set; }

    public DateOnly Date { get; set; }

    public Guid OwnerId { get; protected set; }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if(value < 0)
            {
                throw new SewingFactoryArgumentException(nameof(Quantity));
            }
            _quantity = value;
        }
    }

    public PointOfSale Owner
    {
        get => _owner;
        set => _owner = value ?? throw new SewingFactoryArgumentNullException(nameof(Owner));
    }
}
