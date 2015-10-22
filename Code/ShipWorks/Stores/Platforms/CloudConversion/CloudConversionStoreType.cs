using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.CloudConversion
{
    /// <summary>
    /// Cloud Conversion StoreType
    /// </summary>
    public class CloudConversionStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloudConversionStoreType"/> class.
        /// </summary>
        /// <param name="store"></param>
        public CloudConversionStoreType(StoreEntity store) : base(store)
        {}


        /// <summary>
        /// The numeric type code of the store.
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.CloudConversion;


        /// <summary>
        /// Gets the log source.
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.CloudConversion;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000025166";
    }
}
