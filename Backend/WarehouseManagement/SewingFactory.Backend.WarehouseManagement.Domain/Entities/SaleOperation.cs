using SewingFactory.Backend.WarehouseManagement.Domain.Entities.Base;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities
{
    public sealed class SaleOperation : Operation
    {
        private SaleOperation()  { }

        public SaleOperation(PointOfSale owner, int quantity, DateOnly date) : base(owner, quantity, date)
        {
            
        }
    }
}
