using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;
using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities
{
    public sealed class PointOfSale(string name) : NamedIdentity(name)
    {
        private readonly List<StockItem> _stockItems = [];

        private readonly List<Operation> _operations = [];

        public IReadOnlyList<Operation> Operations => _operations;

        public IReadOnlyList<StockItem> StockItems => _stockItems;

        public StockItem AddStockItem(GarmentModel model)
        {
            if (_stockItems.Any(x => x.GarmentModel.Id == model.Id))
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
            ReduceStockQuantity(quantityToSell, stockItem);
            _operations.Add(new SaleOperation(this, quantityToSell, date));
        }

        public void WriteOff(Guid modelId, int quantityToWriteOff, DateOnly date)
        {
            var stockItem = GetOrThrowStockItemByModelId(modelId);
            ReduceStockQuantity(quantityToWriteOff, stockItem);
            _operations.Add(new WriteOffOperation(this, quantityToWriteOff, date));
        }

        public void Receive(GarmentModel model, int quantityToReceive, DateOnly date)
        {
            var stockItem = _stockItems.FirstOrDefault(x => x.GarmentModel.Id == model.Id);
            if (stockItem is null)
            {
                _stockItems.Add(new StockItem(quantityToReceive, this,model));
            }
            else
            {
                stockItem.Quantity += quantityToReceive;
            }
            _operations.Add(new ReceiveOperation(this, quantityToReceive, date));
        }

        public void Transfer(GarmentModel model, int quantityToTransfer, DateOnly date, PointOfSale receiver)
        {
            var stockItem = GetOrThrowStockItemByModelId(model.Id);
            ReduceStockQuantity(quantityToTransfer, stockItem);
            receiver.Receive(model, quantityToTransfer, date);
            _operations.Add(new InternalTransferOperation(this, quantityToTransfer, date, receiver));
        }

        private static void ReduceStockQuantity(int quantityToSell, StockItem stockItem)
        {
            if (stockItem.Quantity < quantityToSell)
            {
                throw new SewingFactoryInvalidOperationException($"Insufficient stock quantityToSell. Available: {stockItem.Quantity}, requested: {quantityToSell}.");
            }

            stockItem.Quantity -= quantityToSell;
        }

        private StockItem GetOrThrowStockItemByModelId(Guid modelId)
        {
            var stockItem = _stockItems.FirstOrDefault(x => x.GarmentModel.Id == modelId);
            if (stockItem is null)
            {
                throw new SewingFactoryInvalidOperationException($"Stock item for GarmentModel with Id {modelId} does not exist in point of sale {Name}.");
            }

            return stockItem;
        }
    }
}
