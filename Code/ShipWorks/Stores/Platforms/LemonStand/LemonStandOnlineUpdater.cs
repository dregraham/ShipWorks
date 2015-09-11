using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using log4net;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.LemonStand.DTO;
using System.Threading;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    /// Uploads shipment information to LemonStand
    /// </summary>
    public class LemonStandOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(LemonStandOnlineUpdater));

        // the store this instance for
        private readonly LemonStandStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandOnlineUpdater(LemonStandStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public void UpdateShipmentDetails(IEnumerable<long> orderKeys)
        {
            List<string> trackingList = new List<string>();

            LemonStandWebClient client = new LemonStandWebClient(store);

            foreach (long orderKey in orderKeys)
            {
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderKey);
                OrderEntity order = shipment.Order;
                // Check to see if shipment exists
                if (shipment == null)
                {
                    log.InfoFormat("Not uploading orderid {0} has no items.", orderKey);
                    continue;
                }
                string shipmentId = GetShipmentId(client, shipment);

                client.UploadShipmentDetails(shipment.TrackingNumber, shipmentId, order.OnlineStatus, order.OrderNumber.ToString());
            }            
        }

        /// <summary>
        /// Push the online status for an shipment.
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
        /// Gets the shipment id
        /// </summary>
        /// <param name="client">The web client.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>shipment id</returns>
        private string GetShipmentId(LemonStandWebClient client, ShipmentEntity shipmentEntity) 
        {
            LemonStandOrderEntity order = (LemonStandOrderEntity) shipmentEntity.Order;

            string orderId = order.OrderNumber.ToString();
            // use order id to get invoice
            JToken invoice = client.GetOrderInvoice(orderId);
            string invoiceId = invoice.SelectToken("data.invoices.data").Children().First().SelectToken("id").ToString();

            // use invoice id to get shipment
            JToken shipment = client.GetShipment(invoiceId);
            string shipmentId = shipment.SelectToken("data.shipments.data").Children().First().SelectToken("id").ToString();

            return shipmentId;
        }          
    }
}
