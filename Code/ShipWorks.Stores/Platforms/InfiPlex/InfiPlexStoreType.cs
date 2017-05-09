using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.InfiPlex
{
    /// <summary>
    /// InfiPlex store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.InfiPlex, ExternallyOwned=true)]
    public class InfiPlexStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InfiPlexStoreType(StoreEntity store) : base(store)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.InfiPlex;

        /// <summary>
        /// Log request/responses as InfiPlex
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.InfiPlex;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => 
            "http://support.shipworks.com/support/solutions/articles/4000097090-adding-an-infiplex-store";
    }
}
