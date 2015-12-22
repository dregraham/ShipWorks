using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Shopperpress
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class ShopperpressStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopperpressStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Shopperpress;

        /// <summary>
        /// Log request/responses as Shopperpress
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Shopperpress;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022266";
    }
}
