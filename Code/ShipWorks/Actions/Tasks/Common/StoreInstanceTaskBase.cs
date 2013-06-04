using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Base for all ActionTask's that are store instance specific
    /// </summary>
    public abstract class StoreInstanceTaskBase : ActionTask
    {
        long storeID = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        protected StoreInstanceTaskBase()
        {

        }

        /// <summary>
        /// Must be overridden to indicate if the task supports the specified store
        /// </summary>
        public abstract bool SupportsStore(StoreEntity store);

        /// <summary>
        /// The StoreID that the task applies to.
        /// </summary>
        public long StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }
    }
}
