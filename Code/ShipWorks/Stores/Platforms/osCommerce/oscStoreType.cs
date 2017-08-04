using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.osCommerce
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.osCommerce)]
    [Component(RegistrationType.Self)]
    public class oscStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public oscStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.osCommerce;

        /// <summary>
        /// Log request/responses as osCommerce
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.OSCommerce;

        /// <summary>
        /// Gets or sets the account settings help URL.
        /// </summary>
        /// <value>
        /// The account settings help URL.
        /// </value>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/129340";
    }
}
