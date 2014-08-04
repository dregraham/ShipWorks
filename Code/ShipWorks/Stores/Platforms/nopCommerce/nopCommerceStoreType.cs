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
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.nopCommerce;
            }
        }

        /// <summary>
        /// Log request/responses as nopCommerce
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.nopCommerce;
            }
        }

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get
            {
                return "http://support.shipworks.com/solution/articles/4000022738-connecting-nopcommerce-with";
            }
        }
    }
}
