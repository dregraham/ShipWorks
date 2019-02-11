using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Bonanza
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Bonanza)]
    [Component(RegistrationType.Self)]
    public class BonanzaStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BonanzaStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) : 
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// StoreTypeCode enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Bonanza;

        /// <summary>
        /// Log request/response as Bonanza
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Bonanza;

        /// <summary>
        /// Gets the account settings help URL
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022466832";
    }
}