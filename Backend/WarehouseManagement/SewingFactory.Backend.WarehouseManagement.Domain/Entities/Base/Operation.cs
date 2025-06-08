using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;

public abstract class Operation : Identity
{
    private readonly PointOfSale _owner = null!;
    private readonly int _quantity;
    private readonly StockItem _stockStockItem = null!;

    /// <summary>
    ///     default constructor for EF Core
    /// </summary>
    protected Operation()
    {
    }

    protected Operation(PointOfSale owner, int quantity, DateOnly date, StockItem stockItem)
    {
        Owner = owner;
        StockItem = stockItem;
        Quantity = quantity;
        Date = date;
        OwnerId = owner.Id;
        PriceOnOperationDate = stockItem.GarmentModel.Price.Clone();
    }

    public DateOnly Date { get; init; }

    public Money PriceOnOperationDate { get; init; }

    public Guid OwnerId { get; protected init; }

    
    public int Quantity
    {
        get => _quantity;
        init
        {
            if (value < 0)
            {
                throw new SewingFactoryArgumentException(nameof(Quantity));
            }

            _quantity = value;
        }
    }

    public PointOfSale Owner
    {
        get => _owner;
        init => _owner = value ?? throw new SewingFactoryArgumentNullException(nameof(Owner));
    }

    public StockItem StockItem
    {
        get => _stockStockItem;
        init => _stockStockItem = value ?? throw new SewingFactoryArgumentNullException(nameof(StockItem));
    }
    
    public decimal CurrentSum => Quantity * StockItem.GarmentModel.Price.Amount;
    
    public decimal HistoricalSum => Quantity * PriceOnOperationDate.Amount;
    
    public abstract string DisplayName { get; }
}
