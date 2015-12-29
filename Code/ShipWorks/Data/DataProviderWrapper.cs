using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Data
{
    /// <summary>
    /// Central location for obtaining singleton entity cache objects
    /// </summary>
    public class DataProviderWrapper : IDataProvider
    {
        /// <summary>
        /// Get all entities of the given type that are related to the specified entityID
        /// </summary>
        public IEnumerable<EntityBase2> GetRelatedEntities(long orderID, EntityType orderItemEntity) =>
            DataProvider.GetRelatedEntities(orderID, orderItemEntity);
    }
}
