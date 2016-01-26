using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// An active store retrieved from Tango
    /// </summary>
    public class ActiveStore
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
        /// The store type
        /// </summary>
        public string StoreType
        {
            get
            {
                return EnumHelper.GetDescription(new ShipWorksLicense(StoreLicenseKey).StoreTypeCode);
            }
        }
    }
}
