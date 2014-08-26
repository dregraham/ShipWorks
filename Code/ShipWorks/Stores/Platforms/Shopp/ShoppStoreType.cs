using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Shopp
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class ShoppStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShoppStoreType(StoreEntity store) :
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
                return StoreTypeCode.Shopp;
            }
        }

        /// <summary>
        /// Log request/responses as Shopp
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.Shopp;
            }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/solution/categories/105240/folders/261287/articles/4000022263-connecting-to"; }
        }
    }
}
