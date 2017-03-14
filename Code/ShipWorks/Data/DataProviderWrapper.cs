using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Central location for obtaining singleton entity cache objects
    /// </summary>
    [Component]
    public class DataProviderWrapper : IDataProvider, IInitializeForCurrentDatabase, IDisposable
    {
        /// <summary>
        /// Get all entities of the given type that are related to the specified entityID
        /// </summary>
        public IEnumerable<EntityBase2> GetRelatedEntities(long orderID, EntityType orderItemEntity) =>
            DataProvider.GetRelatedEntities(orderID, orderItemEntity);

        /// <summary>
        /// Gets the entity with the given entity ID
        /// </summary>
        public EntityBase2 GetEntity(long entityID, bool fetchIfMissing = true)
            => DataProvider.GetEntity(entityID, fetchIfMissing);

        /// <summary>
        /// Initialize service for the current database
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) =>
            DataProvider.InitializeForCurrentDatabase(executionMode);

        /// <summary>
        /// Gets an entity
        /// </summary>
        public EntityBase2 GetEntity(long entityID) =>
            DataProvider.GetEntity(entityID);

        /// <summary>
        /// Gets the order number complete relating to the given orderID. Returns empty string if order not found.
        /// </summary>
        public string GetOrderNumberComplete(long orderID)
        {
            OrderEntity order = (OrderEntity) GetEntity(orderID);

            return order == null ? string.Empty : order.OrderNumberComplete;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            DataProvider.StopTrackingChanges();
        }
    }
}
