using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using System.Threading;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// An IEntityProvider implementation that uses the standard DataProvider as the EntityProvider
    /// </summary>
    public class DataProviderEntityProvider : IEntityProvider
    {
        EntityType entityType;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataProviderEntityProvider(EntityType entityType)
        {
            this.entityType = entityType;
        }

        /// <summary>
        /// The type of entity this provider returns entities for
        /// </summary>
        public EntityType EntityType
        {
            get { return entityType; }
        }

        /// <summary>
        /// Fetch the entity with the given ID.  If it is not in cache, and fetchIfMissing is true, it will be retrieved from the database.
        /// </summary>
        public EntityBase2 GetEntity(long entityID, bool fetchIfMissing)
        {
            return DataProvider.GetEntity(entityID, fetchIfMissing);
        }

        /// <summary>
        /// Gets all the entities represented by the give keys.  Each key must be of the same EntityType
        /// </summary>
        public List<EntityBase2> GetEntities(List<long> keyList)
        {
            return DataProvider.GetEntities(keyList);
        }
    }
}
