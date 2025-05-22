using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

public class Operation : Identity
{
    private PointOfSale _pointOfSale = null!;
    private int _quantity;

    public Operation(PointOfSale pointOfSale, int quantity, DateOnly date, OperationType operationType)
    {
        PointOfSale = pointOfSale;
        Quantity = quantity;
        Date = date;
        OperationType = operationType;
    }

    public OperationType OperationType { get; set; }

    public DateOnly Date { get; set; }

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

    public PointOfSale PointOfSale
    {
        get => _pointOfSale;
        set => _pointOfSale = value ?? throw new SewingFactoryArgumentNullException(nameof(PointOfSale));
    }
}