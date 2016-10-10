using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using log4net;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// Utility class for updating the online status of ProStores orders
    /// </summary>
    public class ProStoresOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ProStoresOnlineUpdater));
        
        /// <summary>
        /// Upload the shipment details for the given order keys
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
            foreach (ShipmentEntity shipment in shipments)
            {
                if (shipment.Order.IsManual)
                {
                    log.InfoFormat("Not uploading shipment details for shipment {0} because the order is manual.", shipment.ShipmentID);
                    continue;
                }

                if (!shipment.Processed || shipment.Voided)
                {
                    log.InfoFormat("Not uploading tracking number for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                    continue;
                }

                ProStoresStoreEntity store = (ProStoresStoreEntity) StoreManager.GetStore(shipment.Order.StoreID);
                if (store == null)
                {
                    log.WarnFormat("Not uploading shipment details for {0} since the store went away.", shipment.ShipmentID);
                }
                else
                {
                    if (store.LoginMethod == (int) ProStoresLoginMethod.LegacyUserPass)
                    {
                        throw new ProStoresException("Online shipment update is only supported for ProStores version 8.2+ and when using token-based authentication.");
                    }

                    ProStoresWebClient.UploadShipmentDetails(store, shipment);
                }
            }
        }
    }
}
