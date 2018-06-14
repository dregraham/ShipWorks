using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipSense.Population
{
    /// <summary>
    /// No-op implementation of IShipSenseLoaderGateway 
    /// </summary>
    [Component(RegistrationType.Self)]
    public class NullShipSenseLoaderGateway : IShipSenseLoaderGateway
    {
        /// <summary>
        /// Gets the total shipments to analyze when building the knowledge base.
        /// </summary>
        public int TotalShipmentsToAnalyze { get; }

        /// <summary>
        /// Gets the total orders to analyze when populating the hash key of orders.
        /// </summary>
        public int TotalOrdersToAnalyze { get; }

        /// <summary>
        /// Updates the shipment range of the shipping settings that will be used when rebuilding 
        /// the ShipSense knowledge base.
        /// </summary>
        public void ResetShippingSettingsForLoading()
        {
        }

        /// <summary>
        /// Gets the next shipment to analyze based on ShippingSettings
        /// </summary>
        public ShipmentEntity FetchNextShipmentToAnalyze()
        {
            return null;
        }

        /// <summary>
        /// Saves the ShipSenseKnowledgebaseEntity
        /// </summary>
        public void Save(ShipmentEntity shipment)
        {
        }

        /// <summary>
        /// Resets the hash key of all orders to an empty string.
        /// </summary>
        public void ResetOrderHashKeys()
        {
        }

        /// <summary>
        /// Gets an OrderEntity that doesn't have a ShipSenseHashKey
        /// </summary>
        public OrderEntity FetchNextOrderToAnalyze()
        {
            return null;
        }

        /// <summary>
        /// Saves an OrderEntity
        /// </summary>
        public void SaveOrder(OrderEntity order)
        {
        }

        /// <summary>
        /// Gets a sql app lock for loading ShipSense data
        /// </summary>
        /// <param name="appLockName">The name of the lock to take.</param>
        public bool GetAppLock(string appLockName)
        {
            return false;
        }

        /// <summary>
        /// Releases the ShipSense loading sql app lock
        /// </summary>
        /// <param name="appLockName">The name of the lock to release.</param>
        public void ReleaseAppLock(string appLockName)
        {
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}
