using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.InstanteStore
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class InstanteStoreStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InstanteStoreStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.InstaStore;

        /// <summary>
        /// Log request/responses as InstanteStore
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.InstanteStore;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000055044";
    }
}
