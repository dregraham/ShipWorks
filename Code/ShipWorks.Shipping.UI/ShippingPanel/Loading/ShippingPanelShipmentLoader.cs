using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.UI.ShippingPanel.Loading
{
    /// <summary>
    /// Loads a shipment for the order.  If allowed, and no shipment exists, one will be created.  Also validates the shipment addresses.
    /// </summary>
    public class ShippingPanelShipmentLoader : ILoader<ShippingPanelLoadedShipment>
    {
        private readonly IShipmentLoader shipmentLoader;
        private readonly IValidator<ShipmentEntity> addressValidator;

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
            ShippingPanelLoadedShipment shipmentPanelLoadedShipment = await TaskEx.Run(() => shipmentLoader.Load(orderID));

            if (shipmentPanelLoadedShipment.Shipment != null)
            {
                await addressValidator.ValidateAsync(shipmentPanelLoadedShipment.Shipment);
            }

            return shipmentPanelLoadedShipment;
        }
    }
}
