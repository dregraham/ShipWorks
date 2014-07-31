using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.OpenCart
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class OpenCartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenCartStoreType(StoreEntity store) :
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
                return StoreTypeCode.OpenCart;
            }
        }

        /// <summary>
        /// Log request/responses as OpenCart
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.OpenCart;
            }
        }

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/solution/articles/4000022741-connecting-opencart-with"; }
        }
    }
}
