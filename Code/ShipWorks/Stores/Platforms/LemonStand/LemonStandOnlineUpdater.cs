using System.Collections.Generic;
using System.Linq;
using log4net;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    ///     Uploads shipment information to LemonStand
    /// </summary>
    public class LemonStandOnlineUpdater
    {
        // Logger 
        private static readonly ILog log = LogManager.GetLogger(typeof (LemonStandOnlineUpdater));

        // the store this instance is for
        private readonly LemonStandStoreEntity store;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LemonStandOnlineUpdater(LemonStandStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        ///     Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            LemonStandWebClient client = new LemonStandWebClient(store);

            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);

                // Check to see if shipment exists
                if (shipment == null)
                {
                    log.InfoFormat("Not uploading orderid {0} has no items.", orderKey);
                    continue;
                }

                OrderEntity order = shipment.Order;
                string shipmentId = GetShipmentId(client, shipment);

                client.UploadShipmentDetails(shipment.TrackingNumber, shipmentId, order.OnlineStatus,
                    order.OrderNumber.ToString());
            }
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            string tracking = shipment.TrackingNumber;
            OrderEntity order = shipment.Order;
            order.OnlineStatus = "Shipped";

            if (tracking != null)
            {
                LemonStandWebClient client = new LemonStandWebClient(store);
                string shipmentId = GetShipmentId(client, shipment);

                client.UploadShipmentDetails(tracking, shipmentId, order.OnlineStatus, order.OrderNumber.ToString());
            }
        }

        /// <summary>
        ///     Gets the LemonStand shipment ID as it is required to upload tracking information
        /// </summary>
        /// <param name="client">The web client.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>LemonStand API Shipment ID</returns>
        private string GetShipmentId(LemonStandWebClient client, ShipmentEntity shipmentEntity)
        {
            LemonStandOrderEntity order = (LemonStandOrderEntity) shipmentEntity.Order;

            string orderId = order.OrderNumber.ToString();
            // use order id to get invoice
            JToken invoice = client.GetOrderInvoice(orderId);
            string invoiceId = invoice.SelectToken("data.invoices.data").Children().First().SelectToken("id").ToString();

            // use invoice id to get shipment
            JToken shipment = client.GetShipment(invoiceId);
            string shipmentId =
                shipment.SelectToken("data.shipments.data").Children().First().SelectToken("id").ToString();

            return shipmentId;
        }
    }
}