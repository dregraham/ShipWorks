using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Data
{
    /// <summary>
    /// Central location for obtaining singleton entity cache objects
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Get all entities of the given type that are related to the specified entityID
        /// </summary>
        IEnumerable<EntityBase2> GetRelatedEntities(long orderID, EntityType orderItemEntity);

        /// <summary>
        /// Gets the entity with the given ID from cache.  If it does not exist, it is loaded.
        /// </summary>
        EntityBase2 GetEntity(long orderID);
    }
}