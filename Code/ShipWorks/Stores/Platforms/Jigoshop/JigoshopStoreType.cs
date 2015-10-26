using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Jigoshop
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class JigoshopStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JigoshopStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Jigoshop;

        /// <summary>
        /// Log request/responses as Jigoshop
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Jigoshop;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022268";
    }
}
