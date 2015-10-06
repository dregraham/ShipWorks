using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Core.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.ShippingPanel.Loading
{
    /// <summary>
    /// Loads a shipment for the order.  If allowed, and no shipment exists, one will be created.  Also validates the shipment addresses.
    /// </summary>
    public class ShippingPanelShipmentLoader : ILoader<OrderSelectionLoaded>
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
        public async Task<OrderSelectionLoaded> LoadAsync(long orderID)
        {
            OrderSelectionLoaded orderSelectionLoaded = await TaskEx.Run(() => shipmentLoader.Load(orderID));

            if (orderSelectionLoaded.Shipments != null && orderSelectionLoaded.Shipments.Any())
            {
                await addressValidator.ValidateAsync(orderSelectionLoaded.Shipments.FirstOrDefault());
            }

            return orderSelectionLoaded;
        }
    }
}
