using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Zentail
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Zentail)]
    [Component(RegistrationType.Self)]
    public class ZentailStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ZentailStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) : 
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// StoreTypeCode enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Zentail;

        /// <summary>
        /// Log request/response as Zentail
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Zentail;

        /// <summary>
        /// Gets the account settings help URL
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000125142-adding-zentail-to-shipworks";
    }
}