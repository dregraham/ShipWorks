
namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api
{
    public static class CountryCodeCleanser
    {
        /// <summary>
        /// Cleanse the country code for the USPS API
        /// </summary>
        public static string CleanseCountryCode(string code)
        {
            // USPS does not like UK.. only GB
            if (code == "UK")
            {
                code = "GB";
            }

            // Puerto Rico is treated as the United States by USPS
            if (code == "PR" || code == "VI")
            {
                return "US";
            }

            return code;
        }
    }
}
