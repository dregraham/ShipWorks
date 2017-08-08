using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CreLoaded
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.CreLoaded)]
    [Component(RegistrationType.Self)]
    class CreLoadedStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreLoadedStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager)
            : base(store, messageHelper, orderManager)
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

        /// <summary>
        /// Account settings help url
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/129328";
    }
}
