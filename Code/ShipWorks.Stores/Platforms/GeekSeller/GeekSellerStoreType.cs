using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
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
        public GeekSellerStoreType(StoreEntity store) : base(store)
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
