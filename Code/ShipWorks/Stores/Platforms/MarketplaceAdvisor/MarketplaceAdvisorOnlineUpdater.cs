using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using log4net;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Utility class for updating the online information of MA roders
    /// </summary>
    public class MarketplaceAdvisorOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MarketplaceAdvisorOnlineUpdater));

        MarketplaceAdvisorStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOnlineUpdater(MarketplaceAdvisorStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Update the shipment status of the given shipment
        /// </summary>
        public void UpdateShipmentStatus(ShipmentEntity shipment)
        {
            if (shipment.Order.IsManual)
            {
                log.InfoFormat("Not updating order {0} online since its manual.", shipment.Order.OrderID);
                return;
            }

            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return;
            }

            if (store.AccountType == (int) MarketplaceAdvisorAccountType.OMS)
            {
                MarketplaceAdvisorOmsClient.UpdateShipmentStatus(
                    store, 
                    (MarketplaceAdvisorOrderEntity) shipment.Order, 
                    shipment);
            }
            else
            {
                MarketplaceAdvisorLegacyClient client = new MarketplaceAdvisorLegacyClient(store);
                client.UpdateShipmentStatus((MarketplaceAdvisorOrderEntity) shipment.Order, shipment);
            }
        }
    }
}
