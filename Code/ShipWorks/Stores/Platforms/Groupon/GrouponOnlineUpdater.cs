using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using log4net;
using ShipWorks.Data.Model;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Groupon;
using System.Threading;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Uploads shipment details to Groupon
    /// </summary>
    public class GrouponOnlineUpdater
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GrouponOnlineUpdater));

        // the store this instance for
        GrouponStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponOnlineUpdater(GrouponStoreEntity store)
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
                    log.InfoFormat(String.Format("Not uploading shipment details, since the shipment {0} was deleted.", shipmentID));
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
            GrouponWebClient client = new GrouponWebClient(store);
            
            client.UploadShipmentDetails(shipments);
        }
    }
}