using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Uploads shipment details to Amazon
    /// </summary>
    public class AmazonOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonOnlineUpdater));

        // the store this instance for
        private readonly AmazonStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonOnlineUpdater(AmazonStoreEntity store)
        {
            this.store = store;
        }


        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        public void UploadOrderShipmentDetails(IEnumerable<long> orderKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            foreach (long orderID in orderKeys)
            {
                // upload tracking number for the most recent processed, not voided shipment
                ShipmentEntity shipment = OrderUtility.GetLatestActiveShipment(orderID);
                if (shipment == null)
                {
                    log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}.", orderID);
                }
                else
                {
                    shipments.Add(shipment);
                }
            }

            UploadShipmentDetails(shipments);
        }

        /// <summary>
        /// Uploads shipmnent details for a particular shipment
        /// </summary>
        public void UploadShipmentDetails(IEnumerable<long> shipmentKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            foreach (long shipmentID in shipmentKeys)
            {
                ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
                if (shipment == null)
                {
                    log.InfoFormat("Not uploading shipment details, since the shipment {0} was deleted.", shipmentID);
                }
                else
                {
                    shipments.Add(shipment);
                }
            }

            UploadShipmentDetails(shipments);
        }

        /// <summary>
        /// Uploads shipmnent details for a particular shipment
        /// </summary>
        public void UploadShipmentDetails(List<ShipmentEntity> shipments)
        {
            if (store.AmazonApi == (int)AmazonApi.LegacySoap)
            {
                AmazonFeedClient feedClient = new AmazonFeedClient(store);
                feedClient.SubmitFulfillmentFeed(shipments);
            }
            else
            {
                // upload the feed using the MWS API
                using (AmazonMwsClient client = new AmazonMwsClient(store))
                {
                    client.UploadShipmentDetails(shipments);
                }
            }
        }
    }
}