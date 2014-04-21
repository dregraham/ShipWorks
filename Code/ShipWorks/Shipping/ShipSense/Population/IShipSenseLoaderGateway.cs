using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipSense.Population
{
    /// <summary>
    /// Interface for the ShipSenseLoader to get/save database entries
    /// </summary>
    public interface IShipSenseLoaderGateway : IDisposable
    {
        /// <summary>
        /// Gets the ShippingSettingsEntity
        /// </summary>
        ShippingSettingsEntity FetchShippingSettingsEntity();

        /// <summary>
        /// Saves the ShippingSettingsEntity
        /// </summary>
        void SaveShippingSettings(ShippingSettingsEntity shippingSettings);

        /// <summary>
        /// Gets the next shipment to process based on ShippingSettings
        /// </summary>
        ShipmentEntity FetchNextShipmentToProcess();

        /// <summary>
        /// Saves the ShipSenseKnowledgebaseEntity
        /// </summary>
        void Save(ShipmentEntity shipment);

        /// <summary>
        /// Gets an OrderEntity that doesn't have a ShipSenseHashKey
        /// </summary>
        OrderEntity FetchNextOrderOrderToProcess();

        /// <summary>
        /// Saves an OrderEntity
        /// </summary>
        void SaveOrder(OrderEntity order);

        /// <summary>
        /// Gets a sql app lock for loading ShipSense data
        /// </summary>
       /// <param name="appLockName">The name of the lock to take.</param>
       /// <returns>True of the lock could be acquired, false otherwise.</returns>
        bool GetAppLock(string appLockName);

        /// <summary>
        /// Releases the ShipSense loading sql app lock
        /// </summary>
        /// <param name="appLockName">The name of the lock to release.</param>
        void ReleaseAppLock(string appLockName);
    }
}
