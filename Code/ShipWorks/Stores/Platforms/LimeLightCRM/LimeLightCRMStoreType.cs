using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.LimeLightCRM
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class LimeLightCRMStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LimeLightCRMStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.LimeLightCRM;

        /// <summary>
        /// Log request/responses as Lime Light CRM
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.LimeLightCRM;

        /// <summary>
        /// Create a custom web client for Lime Light CRM
        /// </summary>
        public override GenericStoreWebClient CreateWebClient()
        {
            return new LimeLightCRMWebClient(Store as GenericModuleStoreEntity);
        }

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022724";
    }
}
