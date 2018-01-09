using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Filters.Content;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Attribute that can be applied to conditions to specify they are valid for GenericModule storetypes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GenericModuleConditionAttribute : ConditionStoreTypeAttribute
    {
        public override IEnumerable<StoreTypeCode> StoreType
        {
            get
            {
                Type genericModuleType = typeof(GenericModuleStoreType);

                return StoreTypeManager.StoreTypes
                    .Where(storeType => genericModuleType.IsInstanceOfType(storeType))
                    .Select(storeType => storeType.TypeCode);
            }
        }
    }
}