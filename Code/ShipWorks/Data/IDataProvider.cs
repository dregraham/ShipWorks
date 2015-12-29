using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;

namespace ShipWorks.Data
{
    public interface IDataProvider
    {
        IEnumerable<EntityBase2> GetRelatedEntities(long orderID, EntityType orderItemEntity);
    }
}