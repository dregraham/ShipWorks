using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Utility functions for working with the USPS Web Tools
    /// </summary>
    public static class PostalWebUtility
    {
        // Credentials
        private const string uspsUsername = "+e7xG9K15GqfCbfiOelUrQ==";
        private const string uspsPassword = "YbclCwQwTGHqMssnrg+nqA==";

        /// <summary>
        /// The postal server to use when making requests
        /// </summary>
        public static string ServerUrl => UseTestServer ?
            "https://stg-secure.shippingapis.com/ShippingApi.dll" :
            "https://secure.shippingapis.com/ShippingAPI.dll";

        /// <summary>
        /// Indicates if the test server should be used instead of hte live server
        /// </summary>
        public static bool UseTestServer
        {
            get { return InterapptiveOnly.Registry.GetValue("PostalWebTestServer", false); }
            set { InterapptiveOnly.Registry.SetValue("PostalWebTestServer", value); }
        }

        /// <summary>
        /// Username for the USPS servers
        /// </summary>
        public static string UspsUsername => SecureText.Decrypt(uspsUsername, "apptive");

        /// <summary>
        /// Password for the USPS servers
        /// </summary>
        public static string UspsPassword => SecureText.Decrypt(uspsPassword, "apptive");

        /// <summary>
        /// Gets the name of the country.
        /// </summary>
        public static string GetCountryName(string countryCode)
        {
            if (countryCode == "KN")
            {
                return "Saint Kitts";
            }
            if (countryCode == "BA")
            {
                return "Bosnia-Herzegovina";
            }

            return Geography.GetCountryName(countryCode);
        }
    }
}
