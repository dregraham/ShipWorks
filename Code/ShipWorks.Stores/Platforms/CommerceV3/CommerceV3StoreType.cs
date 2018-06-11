using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CommerceV3
{
    [KeyedComponent(typeof(StoreType), StoreTypeCode.CommerceV3)]
    [Component(RegistrationType.Self)]
    public class CommerceV3StoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommerceV3StoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) : 
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// StoreTypeCode enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.CommerceV3;

        /// <summary>
        /// Log request/responses as CommerceV3
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.CommerceV3;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000024104";
    }
}