using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Simple interface for providing entities
    /// </summary>
    public interface IEntityProvider
    {
        /// <summary>
        /// The EntityType that this provider returns entities of
        /// </summary>
        EntityType EntityType { get; }

        /// <summary>
        /// Fetch the entity with the given ID.  If it is not in cache, and fetchIfMissing is true, it will be retrieved from the database.
        /// </summary>
        EntityBase2 GetEntity(long entityID, bool fetchIfMissing = true);

        /// <summary>
        /// Gets all the entities represented by the give keys.  Each key must be of the same EntityType
        /// </summary>
        List<EntityBase2> GetEntities(List<long> keyList);
    }
}
