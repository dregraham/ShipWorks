using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
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

        /// <summary>
        /// Reset the entities dirty fields values to database values
        /// </summary>
        public static void ResetDirtyFieldsToDbValues(this EntityCore<IEntityFields2> entity)
        {
            if (entity.IsDirty)
            {
                entity.Fields.Where(f => f.IsChanged).ForEach(f => f.CurrentValue = f.DbValue);
                entity.IsDirty = false;
            }
        }
    }
}
