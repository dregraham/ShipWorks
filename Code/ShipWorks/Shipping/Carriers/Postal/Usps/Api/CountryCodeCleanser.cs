using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api
{
    /// <summary>
    /// Replace country codes with values that Stamps (USPS) expects
    /// </summary>
    public static class CountryCodeCleanser
    {
        /// <summary>
        /// Cleanse the country code for rating and shipping through USPS
        /// </summary>
        public static string CleanseCountryCode(string code)
        {
            if (code == "UK")
            {
                return "GB";
            }

            return Geography.IsUSInternationalTerritory(code) ? "US" : code;
        }
    }
}
