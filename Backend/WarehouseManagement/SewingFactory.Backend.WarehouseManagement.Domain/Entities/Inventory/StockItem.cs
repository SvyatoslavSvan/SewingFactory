﻿using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations.Base;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;

public sealed class StockItem : Identity
{
    private readonly List<Operation> _operations = null!;
    private GarmentModel _garmentModel = null!;
    private PointOfSale _pointOfSale = null!;
    private int _quantity;

    /// <summary>
    ///     default constructor for EF Core
    /// </summary>
    private StockItem()
    {
    }

    public StockItem(int quantity, PointOfSale pointOfSale, GarmentModel garmentModel)
    {
        Quantity = quantity;
        GarmentModel = garmentModel;
        PointOfSale = pointOfSale;
        ShortageQuantity = 0;
    }


    public PointOfSale PointOfSale
    {
        get => _pointOfSale;
        set => _pointOfSale = value ?? throw new SewingFactoryArgumentNullException(nameof(PointOfSale));
    }

    public GarmentModel GarmentModel
    {
        get => _garmentModel;
        set => _garmentModel = value ?? throw new SewingFactoryArgumentNullException(nameof(GarmentModel));
    }

    public IReadOnlyList<Operation> Operations => _operations;

    public int Quantity
    {
        get => _quantity;
        private set
        {
            if (value < 0)
            {
                throw new SewingFactoryArgumentException(nameof(Quantity), $"{nameof(Quantity)} cannot be negative.");
            }

            _quantity = value;
        }
    }

    public int ShortageQuantity { get; private set; }

    public decimal SumOfStock => (Quantity + ShortageQuantity) * _garmentModel.Price.Amount;
    public void AddOperation(Operation operation) => _operations.Add(operation);

    public void ReduceQuantity(int quantityToReduce)
    {
        if (quantityToReduce <= Quantity)
        {
            Quantity -= quantityToReduce;
            ShortageQuantity = 0;
        }
        else
        {
            ShortageQuantity = quantityToReduce - Quantity;
            Quantity = 0;
        }
    }

    public void IncreaseQuantity(int quantityToIncrease)
    {
        if (quantityToIncrease < 0)
        {
            throw new SewingFactoryArgumentException(nameof(quantityToIncrease), "Quantity to increase cannot be negative.");
        }

        Quantity += quantityToIncrease;
        ShortageQuantity = 0;
    }
}
