using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Attribute that can be applied to conditions to control what storetypes they are valid for
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ConditionStoreTypeAttribute : Attribute
    {
        StoreTypeCode storeType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionStoreTypeAttribute(StoreTypeCode storeType)
        {
            this.storeType = storeType;
        }

        /// <summary>
        /// The StoreType that restricts the filter condition
        /// </summary>
        public StoreTypeCode StoreType
        {
            get { return storeType; }
        }
    }
}
