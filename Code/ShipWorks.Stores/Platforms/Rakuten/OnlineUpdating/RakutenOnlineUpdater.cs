using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Stores.Platforms.Rakuten.DTO;

namespace ShipWorks.Stores.Platforms.Rakuten.OnlineUpdating
{
    /// <summary>
    /// Posts shipping information
    /// </summary>
    [Component]
    public class RakutenOnlineUpdater : IRakutenOnlineUpdater
    {
        private readonly ILog log;
        private readonly IRakutenWebClient updateClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenOnlineUpdater(IRakutenWebClient updateClient,
            Func<Type, ILog> createLogger)
        {
            this.updateClient = updateClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public async Task UploadTrackingNumber(IRakutenStoreEntity store, long shipmentID)
        {
            ShipmentEntity shipment = ShippingManager.GetShipment(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Not uploading tracking number for shipment {0}, order was deleted.", shipmentID);
            }
            else
            {
                await UploadTrackingNumber(store, shipment).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Upload the tracking number of the shipment
        /// </summary>
        public async Task UploadTrackingNumber(IRakutenStoreEntity store, ShipmentEntity shipment)
        {
            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading tracking number since shipment ID {0} is not processed.", shipment.ShipmentID);
                return;
            }

            OrderEntity order = shipment.Order;

            // If the order is manual but also has combined orders, let it through so the updater
            // can check the combined orders manual status.
            if (!order.IsManual || order.CombineSplitStatus.IsCombined())
            {
                try
                {
                    ShippingManager.EnsureShipmentLoaded(shipment);
                }
                catch (ObjectDeletedException)
                {
                    log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                    return;
                }
                catch (SqlForeignKeyException)
                {
                    log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                    return;
                }

                // Upload tracking number
                updateClient.ConfirmShipping(shipment);                                     
            }
            else
            {
                log.InfoFormat("Not uploading tracking number since order {0} is manual.", order.OrderID);
            }
        }
    }
}
