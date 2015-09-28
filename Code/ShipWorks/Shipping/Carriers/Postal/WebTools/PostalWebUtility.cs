using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandRibbon;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Utility functions for working with the USPS Web Tools
    /// </summary>
    public static class PostalWebUtility
    {
        // Credentials
        static string uspsUsername = "+e7xG9K15GqfCbfiOelUrQ==";
        static string uspsPassword = "YbclCwQwTGHqMssnrg+nqA==";

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
        public static string UspsUsername
        {
            get { return SecureText.Decrypt(uspsUsername, "apptive"); }
        }

        /// <summary>
        /// Password for the USPS servers
        /// </summary>
        public static string UspsPassword
        {
            get { return SecureText.Decrypt(uspsPassword, "apptive"); }
        }

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
            else
            {
                return Geography.GetCountryName(countryCode);
            }
        }
    }
}
