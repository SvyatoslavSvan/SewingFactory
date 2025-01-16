using SewingFactory.Common.Domain.Base;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities
{
    public sealed class PointOfSale(string name) : NamedIdentity(name)
    {
        private readonly List<StockItem> _stockItems = [];

        private readonly List<Operation> _operations = [];

        public IReadOnlyList<Operation> Operations => _operations;

        public IReadOnlyList<StockItem> StockItems => _stockItems;

        public void AddStockItem(GarmentModel model) => _stockItems.Add(new StockItem(0,this,model));

        public void Sell(GarmentModel model, int quantityToSell, DateOnly date)
        {
            ReduceStockQuantity(quantityToSell, _stockItems.First(x => x.GarmentModel == model));
            _operations.Add(new Operation(this, quantityToSell, date, OperationType.Sale));
        }

        public void WriteOff(GarmentModel model, int quantityToSell, DateOnly date)
        {
            ReduceStockQuantity(quantityToSell, _stockItems.First(x => x.GarmentModel == model));
            _operations.Add(new Operation(this, quantityToSell, date, OperationType.WriteOff));
        }

        public void Receive(GarmentModel model, int quantityToReceive, DateOnly date)
        {
            var stockItem = _stockItems.First(x => x.GarmentModel.Id == model.Id);
            stockItem.Quantity += quantityToReceive;
            _operations.Add(new Operation(this, quantityToReceive, date, OperationType.Receive));
        }

        public void Transfer(GarmentModel model, int quantityToTransfer, DateOnly date, PointOfSale receiver)
        {
            ReduceStockQuantity(quantityToTransfer, _stockItems.First(x => x.GarmentModel == model));
            receiver.Receive(model, quantityToTransfer, date);
            _operations.Add(new InternalTransferOperation(this, quantityToTransfer, date, receiver));
        }

        private static void ReduceStockQuantity(int quantityToSell, StockItem stockItem)
        {
            if (stockItem.Quantity < quantityToSell)
            {
                throw new InvalidOperationException($"Insufficient stock quantityToSell. Available: {stockItem.Quantity}, requested: {quantityToSell}.");
            }

            stockItem.Quantity -= quantityToSell;
        }
    }
}
