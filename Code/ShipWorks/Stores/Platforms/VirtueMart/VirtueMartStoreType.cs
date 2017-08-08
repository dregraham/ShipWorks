using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.VirtueMart
{
    /// <summary>
    /// VirtueMart integration
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.VirtueMart)]
    [Component(RegistrationType.Self)]
    class VirtueMartStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VirtueMartStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// Store Type
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.VirtueMart;

        /// <summary>
        /// Log request/responses as VirtueMart
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.VirtueMart;

        /// <summary>
        /// Gets or sets the account settings help URL.
        /// </summary>
        /// <value>
        /// The account settings help URL.
        /// </value>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/129343";
    }
}
