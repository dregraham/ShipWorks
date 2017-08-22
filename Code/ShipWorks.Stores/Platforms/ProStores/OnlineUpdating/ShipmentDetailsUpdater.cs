using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Enums;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ProStores.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// Utility class for updating the online status of ProStores orders
    /// </summary>
    public class ShipmentDetailsUpdater : IShipmentDetailsUpdater
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDetailsUpdater(Func<Type, ILog> createLogger)
        {
            log = createLogger(GetType());
        }

        /// <summary>
        /// Upload the shipment details for the given order keys
        /// </summary>
        public async Task UploadOrderShipmentDetails(IEnumerable<long> orderKeys)
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

            await UploadShipmentDetails(shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(IEnumerable<long> shipmentKeys)
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

            await UploadShipmentDetails(shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(List<ShipmentEntity> shipments)
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                if (shipment.Order.IsManual && shipment.Order.CombineSplitStatus != CombineSplitStatusType.Combined)
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
                    continue;
                }

                if (store.LoginMethod == (int) ProStoresLoginMethod.LegacyUserPass)
                {
                    throw new ProStoresException("Online shipment update is only supported for ProStores version 8.2+ and when using token-based authentication.");
                }

                await ProStoresWebClient.UploadShipmentDetails(store, shipment).ConfigureAwait(false);
            }
        }
    }
}
