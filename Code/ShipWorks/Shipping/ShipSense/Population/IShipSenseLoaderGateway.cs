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
        /// Gets the total shipments to analyze when building the knowledge base.
        /// </summary>
        int TotalShipmentsToAnalyze { get; }

        /// <summary>
        /// Gets the total orders to analyze when populating the hash key of orders.
        /// </summary>
        int TotalOrdersToAnalyze { get; }

        /// <summary>
        /// Gets the next shipment to analyze based on ShippingSettings
        /// </summary>
        ShipmentEntity FetchNextShipmentToAnalyze();

        /// <summary>
        /// Saves the ShipSenseKnowledgebaseEntity
        /// </summary>
        void Save(ShipmentEntity shipment);

        /// <summary>
        /// Resets the hash key of all orders to an empty string.
        /// </summary>
        void ResetOrderHashKeys();

        /// <summary>
        /// Gets an OrderEntity that doesn't have a ShipSenseHashKey
        /// </summary>
        OrderEntity FetchNextOrderToAnalyze();
        
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
