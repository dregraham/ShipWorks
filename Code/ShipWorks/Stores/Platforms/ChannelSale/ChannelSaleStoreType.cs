using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.ChannelSale
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class ChannelSaleStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelSaleStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.ChannelSale;
            }
        }

        /// <summary>
        /// Log request/responses as CreLoaded
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.ChannelSale;
            }
        }

        /// <summary>
        /// Use the username, since the integration url will be the same for all customers.
        /// </summary>
        protected override string InternalLicenseIdentifier
        {
            get
            {   
                GenericModuleStoreEntity genericStore = (GenericModuleStoreEntity)Store;
                return genericStore.ModuleUsername;
            }
        }
    }
}
