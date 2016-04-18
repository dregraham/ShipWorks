using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// An active store retrieved from Tango
    /// </summary>
    public class ActiveStore : IActiveStore
    {
        /// <summary>
        /// The store license key.
        /// </summary>
        public string StoreLicenseKey { get; set; }

        /// <summary>
        /// The store name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The store icon
        /// </summary>
        public StoreTypeCode StoreType => new ShipWorksLicense(StoreLicenseKey).StoreTypeCode;
    }
}
