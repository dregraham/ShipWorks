using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.LimeLightCRM
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.LimeLightCRM)]
    [Component(RegistrationType.Self)]
    public class LimeLightCRMStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LimeLightCRMStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
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
