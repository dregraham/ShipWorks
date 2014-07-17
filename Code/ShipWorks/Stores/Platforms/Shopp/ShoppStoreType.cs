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
    }
}
