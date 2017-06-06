using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SellerCloud
{
    /// <summary>
    /// SellerCloud generic module store implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SellerCloud, ExternallyOwned=true)]
    public class SellerCloudStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerCloudStoreType(StoreEntity store) : base(store)
        {
        }

        /// <summary>
        /// StoreTypeCode enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SellerCloud;

        /// <summary>
        /// Log request/responses as SellerCloud
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SellerCloud;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => 
            "http://support.shipworks.com/support/solutions/articles/4000097089-adding-a-sellercloud-store";
    }
}
