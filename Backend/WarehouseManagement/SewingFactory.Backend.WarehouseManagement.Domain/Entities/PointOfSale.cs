using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities;

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
    {
        var stockItem = GetOrThrowStockItemByModelId(modelId);
        stockItem.ReduceQuantity(quantityToSell);
        _operations.Add(new SaleOperation(this, quantityToSell, date, stockItem));
    }

    public void WriteOff(Guid modelId, int quantityToWriteOff, DateOnly date)
    {
        var stockItem = GetOrThrowStockItemByModelId(modelId);
        stockItem.ReduceQuantity(quantityToWriteOff);
        _operations.Add(new WriteOffOperation(this, quantityToWriteOff, date, stockItem));
    }

    public void Receive(GarmentModel model, int quantityToReceive, DateOnly date)
    {
        var stockItem = _stockItems.FirstOrDefault(predicate: x => x.GarmentModel.Id == model.Id);
        if (stockItem is null)
        {
            stockItem = new StockItem(quantityToReceive, this, model);
            _stockItems.Add(stockItem);
        }
        else
        {
            stockItem.IncreaseQuantity(quantityToReceive);
        }

        _operations.Add(new ReceiveOperation(this, quantityToReceive, date, stockItem));
    }

    public void Transfer(GarmentModel model, int quantityToTransfer, DateOnly date, PointOfSale receiver)
    {
        var stockItem = GetOrThrowStockItemByModelId(model.Id);
        stockItem.ReduceQuantity(quantityToTransfer);
        receiver.Receive(model, quantityToTransfer, date);
        _operations.Add(new InternalTransferOperation(this, quantityToTransfer, date, receiver, stockItem));
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
    
    
}
