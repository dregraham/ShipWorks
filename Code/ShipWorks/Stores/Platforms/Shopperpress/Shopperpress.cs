using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Shopperpress
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Shopperpress)]
    [Component(RegistrationType.Self)]
    public class ShopperpressStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopperpressStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Shopperpress;

        /// <summary>
        /// Log request/responses as Shopperpress
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Shopperpress;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022266";
    }
}
