using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Class to delegate tasks on previous shipments (reprint, void, etc)
    /// </summary>
    [Component]
    public class PreviousShipmentReprintActionHandler : IPreviousShipmentReprintActionHandler
    {
        private readonly IMessenger messenger;
        private readonly IShippingManager shippingManager;
        private readonly IOrderLookupPreviousShipmentLocator shipmentLocator;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public PreviousShipmentReprintActionHandler(
            IMessenger messenger,
            IOrderLookupPreviousShipmentLocator shipmentLocator,
            IShippingManager shippingManager,
            IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.shipmentLocator = shipmentLocator;
            this.messenger = messenger;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Reprint the last shipment
        /// </summary>
        public async Task ReprintLastShipment()
        {
            var shipmentDetails = await shipmentLocator.GetLatestShipmentDetails().ConfigureAwait(true);

            if (shipmentDetails == null)
            {
                messageHelper.ShowError("Could not find a processed shipment from today");
                return;
            }

            ShipmentEntity shipment = new ShipmentEntity(shipmentDetails.ShipmentID);

            try
            {
                shippingManager.RefreshShipment(shipment);

                if (shipment.Voided)
                {
                    messageHelper.ShowError("Cannot reprint label for the last shipment because it has been voided");
                }
                else
                {
                    if (shipment.Processed)
                    {
                        messenger.Send(new ReprintLabelsMessage(this, new[] { shipment }), string.Empty);
                    }
                }
            }
            catch (ObjectDeletedException)
            {
                // Just continue
            }
        }
    }
}
