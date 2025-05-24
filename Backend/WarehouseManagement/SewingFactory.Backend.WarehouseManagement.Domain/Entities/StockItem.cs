using SewingFactory.Common.Domain.Base;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Backend.WarehouseManagement.Domain.Entities
{
    public sealed class StockItem : Identity
    {
        private PointOfSale _pointOfSale = null!;
        private GarmentModel _garmentModel = null!;
        private int _quantity;

        /// <summary>
        /// default constructor for EF Core
        /// </summary>
        private StockItem()
        {
            
        }

        public StockItem(int quantity, PointOfSale pointOfSale, GarmentModel garmentModel)
        {
            Quantity = quantity;
            GarmentModel = garmentModel;
            PointOfSale = pointOfSale;
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

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                {
                    throw new SewingFactoryArgumentException(nameof(Quantity), "Quantity cannot be negative");
                }
                _quantity = value;
            }
        }
    }
}
