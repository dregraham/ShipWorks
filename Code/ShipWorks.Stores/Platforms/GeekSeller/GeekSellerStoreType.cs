using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.GeekSeller
{
    /// <summary>
    /// GeekSeller store type
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.GeekSeller, ExternallyOwned = true)]
    public class GeekSellerStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store"></param>
        public GeekSellerStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.GeekSeller;

        /// <summary>
        /// Log request/responses as PowersportsSupport
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.GeekSeller;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl =>
            "http://support.shipworks.com/support/solutions/articles/4000101692-adding-a-geekseller-store";

        /// <summary>
        /// Create user control for configuring the tasks.
        /// This is customized because it is desired to auto set the complete status
        /// </summary>
        public override OnlineUpdateActionControlBase CreateAddStoreWizardOnlineUpdateActionControl()
        {
            return new GeekSellerOnlineUpdateActionControl();
        }
    }
}
