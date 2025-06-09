using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Garment;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations;
using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Operations.Base;
using SewingFactory.Backend.WarehouseManagement.Domain.Enums;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities.Inventory;

public sealed class PointOfSale(string name) : NamedIdentity(name)
{
    private readonly List<Operation> _operations = [];
    private readonly List<StockItem> _stockItems = [];

    public IReadOnlyList<Operation> Operations => _operations;

    public IReadOnlyList<StockItem> StockItems => _stockItems;

    public StockItem AddStockItem(GarmentModel model)
    {
        if (_stockItems.Any(predicate: x => x.GarmentModel.Id == model.Id))
        {
            throw new SewingFactoryInvalidOperationException($"Stock item for model {model.Name} already exists in point of sale {Name}.");
        }

        var stockItem = new StockItem(0, this, model);
        _stockItems.Add(stockItem);

        return stockItem;
    }


    public void Sell(Guid modelId, int quantityToSell, DateOnly date)
        => DoOperation(modelId, quantityToSell, operationFactory: stockItem => new SaleOperation(this, quantityToSell, date, stockItem), StockFlow.Outcome);

    public void WriteOff(Guid modelId, int quantityToWriteOff, DateOnly date)
        => DoOperation(modelId, quantityToWriteOff, operationFactory: item => new WriteOffOperation(this, quantityToWriteOff, date, item), StockFlow.Outcome);

    public void Receive(GarmentModel model, int quantityToReceive, DateOnly date)
    {
        var stockItem = FindOrCreateStockItem(model);
        DoOperation(model.Id, quantityToReceive, operationFactory: _ => new ReceiveOperation(this, quantityToReceive, date, stockItem), StockFlow.Income);
    }

    public void Transfer(GarmentModel model, int quantityToTransfer, DateOnly date, PointOfSale receiver)
    {
        DoOperation(model.Id, quantityToTransfer, operationFactory: stockItem => new InternalTransferOperation(this, quantityToTransfer, date, receiver, stockItem), StockFlow.Outcome);
        receiver.Receive(model, quantityToTransfer, date);
    }

    private void DoOperation(
        Guid garmentModelId,
        int quantity,
        Func<StockItem, Operation> operationFactory,
        StockFlow flow)
    {
        var stockItem = GetOrThrowStockItemByModelId(garmentModelId);
        if (flow == StockFlow.Income)
        {
            stockItem.IncreaseQuantity(quantity);
        }
        else if (flow == StockFlow.Outcome)
        {
            stockItem.ReduceQuantity(quantity);
        }

        _operations.Add(operationFactory(stockItem));
    }

    private StockItem GetOrThrowStockItemByModelId(Guid modelId)
    {
        var stockItem = _stockItems.FirstOrDefault(predicate: x => x.GarmentModel.Id == modelId);
        if (stockItem is null)
        {
            throw new SewingFactoryInvalidOperationException($"Stock item for GarmentModel with Id {modelId} does not exist in point of sale {Name}.");
        }

        return stockItem;
    }

    private StockItem FindOrCreateStockItem(GarmentModel model)
    {
        var stockItem = _stockItems.FirstOrDefault(predicate: x => x.GarmentModel.Id == model.Id);
        if (stockItem is not null)
        {
            return stockItem;
        }

        stockItem = new StockItem(0, this, model);
        _stockItems.Add(stockItem);

        return stockItem;
    }
}
