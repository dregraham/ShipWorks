using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Utility class for updating the online information of MA roders
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public class MarketplaceAdvisorOnlineUpdater
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MarketplaceAdvisorOnlineUpdater));

        private readonly MarketplaceAdvisorStoreEntity store;

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
                MarketplaceAdvisorOmsClient.Create(store).UpdateShipmentStatus(
                    new MarketplaceAdvisorOrderDto(shipment.Order),
                    new MarketplaceAdvisorShipmentDto(shipment));
            }
            else
            {
                MarketplaceAdvisorLegacyClient client = MarketplaceAdvisorLegacyClient.Create(store);
                client.UpdateShipmentStatus(new MarketplaceAdvisorOrderDto(shipment.Order), new MarketplaceAdvisorShipmentDto(shipment));
            }
        }
    }
}
