using ShipWorks.Data.Model.EntityClasses;


namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Base for all ActionTask's that are store instance specific
    /// </summary>
    public abstract class StoreInstanceTaskBase : ActionTask
    {
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
        public long StoreID { get; set; } = -1;
    }
}
