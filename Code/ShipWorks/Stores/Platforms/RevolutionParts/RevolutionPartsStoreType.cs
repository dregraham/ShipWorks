using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.RevolutionParts
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class RevolutionPartsStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RevolutionPartsStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.RevolutionParts;

        /// <summary>
        /// Log request/responses as RevolutionParts
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.RevolutionParts;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000065048";
    }
}
