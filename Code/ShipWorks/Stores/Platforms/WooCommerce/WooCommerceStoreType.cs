using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.WooCommerce
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class WooCommerceStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WooCommerceStoreType(StoreEntity store) : 
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.WooCommerce;

        /// <summary>
        /// Log request/responses as WooCommerce
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.WooCommerce;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022264";
    }
}
