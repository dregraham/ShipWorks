﻿using System;
using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model;

namespace ShipWorks.Data
{
    /// <summary>
    /// Central location for obtaining singleton entity cache objects
    /// </summary>
    public class DataProviderWrapper : IDataProvider, IInitializeForCurrentDatabase, IDisposable
    {
        /// <summary>
        /// Get all entities of the given type that are related to the specified entityID
        /// </summary>
        public IEnumerable<EntityBase2> GetRelatedEntities(long orderID, EntityType orderItemEntity) =>
            DataProvider.GetRelatedEntities(orderID, orderItemEntity);

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
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            DataProvider.StopTrackingChanges();
        }
    }
}
