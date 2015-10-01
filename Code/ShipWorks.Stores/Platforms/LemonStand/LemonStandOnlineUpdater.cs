using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    ///     Uploads shipment information to LemonStand
    /// </summary>
    public class LemonStandOnlineUpdater
    {
        // the store this instance is for
        private readonly ILemonStandWebClient client;
        // Logger 
        private readonly ILog log;

        /// <summary>
        ///     Constructor
        /// </summary>
        public LemonStandOnlineUpdater(LemonStandStoreEntity store)
            : this(LogManager.GetLogger(typeof (LemonStandOnlineUpdater)), new LemonStandWebClient(store))
        {
        }

        public LemonStandOnlineUpdater(ILog log, ILemonStandWebClient client)
        {
            this.log = log;
            this.client = client;
        }

        /// <summary>
        ///     Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);

                // Check to see if shipment exists
                if (shipment == null)
                {
                    log.InfoFormat("Not uploading orderid {0} has no items.", orderKey);
                    continue;
                }

                LemonStandOrderEntity order = (LemonStandOrderEntity) shipment.Order;
                if (!order.IsManual)
                {
                    string shipmentID = GetShipmentID(shipment);

                    client.UploadShipmentDetails(shipment.TrackingNumber, shipmentID, order.OnlineStatus,
                        order.LemonStandOrderID);
                }
            }
        }

        /// <summary>
        ///     Push the online status for an shipment.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            LemonStandOrderEntity order = (LemonStandOrderEntity)shipment.Order;
            order.OnlineStatus = "Shipped";

            if (!order.IsManual)
            {
                string shipmentID = GetShipmentID(shipment);

                client.UploadShipmentDetails(shipment.TrackingNumber, shipmentID, order.OnlineStatus,
                    order.LemonStandOrderID);
            }
        }

        /// <summary>
        ///     Gets the LemonStand shipment ID as it is required to upload tracking information
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>LemonStand API Shipment ID</returns>
        private string GetShipmentID(ShipmentEntity shipmentEntity)
        {
            LemonStandOrderEntity order = (LemonStandOrderEntity) shipmentEntity.Order;

            string orderID = order.OrderNumber.ToString();
            // use order id to get invoice
            JToken invoice = client.GetOrderInvoice(orderID);
            string invoiceID = invoice.SelectToken("data.invoices.data").Children().First().SelectToken("id").ToString();

            // use invoice id to get shipment
            JToken shipment = client.GetShipment(invoiceID);
            string shipmentID =
                shipment.SelectToken("data.shipments.data").Children().First().SelectToken("id").ToString();

            return shipmentID;
        }
    }
}