using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SellerActive
{
    /// <summary>
    /// SellerActive generic module store implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SellerActive, ExternallyOwned = true)]
    public class SellerActiveStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerActiveStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreTypeCode enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SellerActive;

        /// <summary>
        /// Log request/responses as SellerActive
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SellerActive;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl =>
            "http://support.shipworks.com/support/solutions/articles/4000098529-adding-a-selleractive-store";
    }
}
