using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// Translate the given list ID into all the keys they relate to of the given target type
        /// </summary>
        IEnumerable<long> GetRelatedKeys(long id, EntityType relateToType);

        /// <summary>
        /// Gets the entity with the given entity ID
        /// </summary>
        EntityBase2 GetEntity(long entityID, bool fetchIfMissing = true);

        /// <summary>
        /// Gets the entity with the given entity ID
        /// </summary>
        Task<T> GetEntityAsync<T>(long entityID) where T : EntityBase2;

        /// <summary>
        /// Gets the entity with the given entity ID
        /// </summary>
        Task<T> GetEntityAsync<T>(long entityID, bool fetchIfMissing) where T : EntityBase2;

        /// <summary>
        /// Gets the order number complete relating to the given orderID. Returns empty string if order not found.
        /// </summary>
        string GetOrderNumberComplete(long orderID);
    }
}