using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.OrderDesk
{
    /// <summary>
    /// OrderDeskCart Store Type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.OrderDesk)]
    [Component(RegistrationType.Self)]
    public class OrderDeskStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDeskStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        public OrderDeskStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.OrderDesk;

        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.OrderDeskCart;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000022320";
    }
}
