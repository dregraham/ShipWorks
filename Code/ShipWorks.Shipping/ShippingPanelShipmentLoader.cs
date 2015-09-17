using System;
using System.Threading.Tasks;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Loads a shipment for the order.  If allowed, and no shipment exists, one will be created.  Also validates the shipment addresses.
    /// </summary>
    public class ShippingPanelShipmentLoader : ILoader<ShippingPanelLoadedShipment>
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingPanelShipmentLoader));

        private IShipmentLoader shipmentLoader;
        private IValidator<ShipmentEntity> addressValidator;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShippingPanelShipmentLoader(IShipmentLoader shipmentLoader, IValidator<ShipmentEntity> addressValidator)
        {
            this.shipmentLoader = shipmentLoader;
            this.addressValidator = addressValidator;
        }

        /// <summary>
        /// Load the shipment results asychronously.
        /// </summary>
        public async Task<ShippingPanelLoadedShipment> LoadAsync(long orderID)
        {
            ShippingPanelLoadedShipment shipmentPanelLoadedShipment = shipmentLoader.Load(orderID);

            if (shipmentPanelLoadedShipment.Shipment != null)
            {
                await addressValidator.ValidateAsync(shipmentPanelLoadedShipment.Shipment);
            }

            return shipmentPanelLoadedShipment;
        }
    }
}
