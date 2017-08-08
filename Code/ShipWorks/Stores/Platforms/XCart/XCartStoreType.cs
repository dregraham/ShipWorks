using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.XCart
{
    /// <summary>
    /// X-Cart integration point
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.XCart)]
    [Component(RegistrationType.Self)]
    class XCartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public XCartStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// Gets the code to identify this store as XCart
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.XCart;

        /// <summary>
        /// Log request/responses as XCart
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.XCart;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000029976";
    }
}
