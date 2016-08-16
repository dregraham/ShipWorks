using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.CreLoaded
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    class CreLoadedStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreLoadedStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.CreLoaded;

        /// <summary>
        /// Log request/responses as CreLoaded
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.CreLoaded;

        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/129328";
    }
}
