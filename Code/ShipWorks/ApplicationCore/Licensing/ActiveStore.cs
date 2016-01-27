using Interapptive.Shared.Utility;
using ShipWorks.Stores;
using System.Drawing;

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
        /// The store icon
        /// </summary>
        public StoreTypeCode StoreType
        {
            get
            {
                return new ShipWorksLicense(StoreLicenseKey).StoreTypeCode;
            }
        }
    }
}
