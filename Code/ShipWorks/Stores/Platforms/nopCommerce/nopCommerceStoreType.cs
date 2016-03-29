using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.nopCommerce
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class nopCommerceStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public nopCommerceStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.nopCommerce;

        /// <summary>
        /// Log request/responses as nopCommerce
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.nopCommerce;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022738";
    }
}
