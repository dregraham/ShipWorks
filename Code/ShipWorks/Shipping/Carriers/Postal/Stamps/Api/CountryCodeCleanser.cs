
namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Api
{
    public static class CountryCodeCleanser
    {
        /// <summary>
        /// Cleanse the country code for the Stamps.com API
        /// </summary>
        public static string CleanseCountryCode(string code)
        {
            // Stamps.com does not like UK.. only GB
            if (code == "UK")
            {
                code = "GB";
            }

            // Puerto Rico is treated as the United States by Stamps
            if (code == "PR" || code == "VI")
            {
                return "US";
            }

            return code;
        }
    }
}
