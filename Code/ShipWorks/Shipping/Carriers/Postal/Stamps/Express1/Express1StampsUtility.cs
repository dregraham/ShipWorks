using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Helper methods for the Stamps.com version of Express1
    /// </summary>
    static class Express1StampsUtility
    {
        /// <summary>
        /// Determines if the Live Server should be used
        /// </summary>
        public static bool UseTestServer
        {
            get
            {
                return InterapptiveOnly.Registry.GetValue("Express1StampsTestServer", false);
            }
            set
            {
                InterapptiveOnly.Registry.SetValue("Express1StampsTestServer", value);
            }
        }
    }
}
