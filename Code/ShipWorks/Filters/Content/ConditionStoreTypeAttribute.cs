using System;
using System.Collections.Generic;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Attribute that can be applied to conditions to control what storetypes they are valid for
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ConditionStoreTypeAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionStoreTypeAttribute()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionStoreTypeAttribute(StoreTypeCode storeType)
        {
            StoreType = storeType;
        }

        /// <summary>
        /// The StoreType that restricts the filter condition
        /// </summary>
        public virtual StoreTypeCode StoreType { get; }
    }
}
