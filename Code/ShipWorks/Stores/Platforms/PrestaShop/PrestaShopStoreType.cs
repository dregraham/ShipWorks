using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.PrestaShop
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class PrestaShopStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PrestaShopStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.PrestaShop;

        /// <summary>
        /// Log request/responses as PrestaShop
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.PrestaShop;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000043804";
    }
}
