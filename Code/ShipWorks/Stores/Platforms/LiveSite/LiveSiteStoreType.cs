using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.LiveSite
{
    /// <summary>
    /// LiveSite StoreType
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.LiveSite)]
    [Component(RegistrationType.Self)]
    internal class LiveSiteStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveSiteStoreType"/> class.
        /// </summary>
        public LiveSiteStoreType(StoreEntity store)
            : base(store)
        { }

        /// <summary>
        /// Gets the type code for this store
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.LiveSite;

        /// <summary>
        /// Get the log source
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.LiveSite;

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl =>
            "http://support.shipworks.com/solution/articles/4000022404-connecting-livesite-with";
    }
}
