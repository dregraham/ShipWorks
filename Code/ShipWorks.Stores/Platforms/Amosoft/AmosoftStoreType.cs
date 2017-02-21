using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Amosoft
{
    /// <summary>
    /// Amosoft generic module store implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Amosoft, ExternallyOwned = true)]
    public class AmosoftStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmosoftStoreType"/> class.
        /// </summary>
        public AmosoftStoreType(StoreEntity store) : base(store)
        {
        }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Amosoft;

        /// <summary>
        /// Gets the log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Amosoft;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl =>
            "http://support.shipworks.com/support/solutions/articles/4000097763-adding-an-amosoft-store";
    }
}