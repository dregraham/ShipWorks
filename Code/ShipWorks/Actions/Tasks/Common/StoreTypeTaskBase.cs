using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Base for all tasks that are store-type based
    /// </summary>
    public abstract class StoreTypeTaskBase : ActionTask
    {
        StoreTypeCode storeTypeCode = StoreTypeCode.Invalid;

        /// <summary>
        /// Constructor
        /// </summary>
        protected StoreTypeTaskBase()
        {

        }

        /// <summary>
        /// Must be overridden to indicate if the task supports the specified store
        /// </summary>
        public abstract bool SupportsType(StoreType storeType);

        /// <summary>
        /// The store type code that the task applies to.
        /// </summary>
        public StoreTypeCode StoreTypeCode
        {
            get { return storeTypeCode; }
            set { storeTypeCode = value; }
        }
    }
}
