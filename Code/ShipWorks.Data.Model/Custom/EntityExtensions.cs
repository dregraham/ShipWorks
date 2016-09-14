using System.Collections.Generic;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// Extensions for entity related things
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Create an entity collection for a given enumerable
        /// </summary>
        public static EntityCollection<T> ToEntityCollection<T>(this IEnumerable<T> source) where T : EntityBase2
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));

            EntityCollection<T> collection = new EntityCollection<T>();
            collection.AddRange(source);
            return collection;
        }
    }
}
