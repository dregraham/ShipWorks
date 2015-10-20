using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.PowersportsSupport
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class PowersportsSupportStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PowersportsSupportStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.PowersportsSupport;

        /// <summary>
        /// Log request/responses as PowersportsSupport
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.PowersportsSupport;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000024104";
    }
}
