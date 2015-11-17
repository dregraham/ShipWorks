using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Interapptive.Shared.Business.Geography
{
    /// <summary>
    /// Maintains countries, states, and provinces
    /// </summary>
    public static class Geography
    {
        static SortedList<string, string> states = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        static SortedList<string, string> provinces = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        static SortedList<string, string> countriesWithNoStateProvinces = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Static contructor
        /// </summary>
        static Geography()
        {
            LoadStates();
            LoadProvinces();
            LoadCountriesWithNoStateProvinces();
        }

        /// <summary>
        /// Returns a readonly list of Country names
        /// </summary>
        public static IList<string> Countries
        {
            get { return CountryList.Countries; }
        }

        /// <summary>
        /// Get the code of the country with the given name.  If not found, the original name is returned.
        /// </summary>
        public static string GetCountryCode(string name)
        {
            return CountryList.GetCountryCode(name);
        }

        /// <summary>
        /// Get the name of the country based on the given country code.  If not found, the original code is returned.
        /// </summary>
        public static string GetCountryName(string name)
        {
            return CountryList.GetCountryName(name);
        }

        /// <summary>
        /// Get the icon image of the country flag. Null if one is not present for the country.
        /// </summary>
        public static Image GetCountryFlag(string code)
        {
            return CountryFlags.GetCountryFlag(code);
        }

        /// <summary>
        /// Returns a readonly list of State names
        /// </summary>
        public static IList<string> States
        {
            get { return new List<string>(states.Keys); }
        }

        /// <summary>
        /// Returns a readonly list of Province names
        /// </summary>
        public static IList<string> Provinces
        {
            get { return new List<string>(provinces.Keys); }
        }

        /// <summary>
        /// Get the code of the state or province name.  If not found, the original name is returned.
        /// </summary>
        public static string GetStateProvCode(string name)
        {
            return GetStateProvCode(name, string.Empty);
        }

        /// <summary>
        /// Get the code of the state or province name.  If not found, the original name is returned.
        /// If countryCode is a country that does not have states/provinces, string.Empty is returned.
        /// </summary>
        public static string GetStateProvCode(string name, string countryCode)
        {
            // If we get a null (which we've seen from ebay at least), just return an empty string
            if (String.IsNullOrEmpty(name))
            {
                return String.Empty;
            }

            // If the state/prov is for a country that doesn't have states or provinces, just return string.Empty
            if (countriesWithNoStateProvinces.ContainsKey(countryCode) ||
                countriesWithNoStateProvinces.ContainsValue(countryCode))
            {
                return string.Empty;
            }

            string code;
            if (states.TryGetValue(name, out code))
            {
                return code;
            }

            if (provinces.TryGetValue(name, out code))
            {
                return code;
            }

            // Test for the accented e
            if (Regex.Match(name, "qu.bec", RegexOptions.IgnoreCase).Success)
            {
                return "QC";
            }

            // Since Puerto Rico is not included in the list of states, we need to handle it separately
            if (name.Equals("puerto rico", StringComparison.OrdinalIgnoreCase))
            {
                return "PR";
            }

            // Check for lowercase codes
            if (name.Length <= 2)
            {
                return name.ToUpperInvariant();
            }
 
            return name;
        }

        /// <summary>
        /// Gets the name of a state/province without regard for country
        /// </summary>
        public static string GetStateProvName(string code)
        {
            return GetStateProvName(code, null);
        }

        /// <summary>
        /// Get the name of the state or province based on the code.  If not found, the original code is returned.
        /// </summary>
        public static string GetStateProvName(string code, string countryCode)
        {
            int index;
            if (countryCode == null || String.Compare(countryCode, "US", StringComparison.OrdinalIgnoreCase) == 0)
            {
                index = states.IndexOfValue(code);
                if (index != -1)
                {
                    return states.Keys[index];
                }
            }

            if (countryCode == null || String.Compare(countryCode, "CA", StringComparison.OrdinalIgnoreCase) == 0)
            {
                index = provinces.IndexOfValue(code);
                if (index != -1)
                {
                    return provinces.Keys[index];
                }
            }

            // Since Puerto Rico is not included in the list of states, we need to handle it separately
            return code.Equals("PR", StringComparison.OrdinalIgnoreCase) ? "Puerto Rico" : code;
        }

        #region Loading

        /// <summary>
        /// Load the state list
        /// </summary>
        private static void LoadStates()
        {
            states.Add("Alabama", "AL");
            states.Add("Alaska", "AK");
            states.Add("Arizona", "AZ");
            states.Add("Arkansas", "AR");
            states.Add("California", "CA");
            states.Add("Colorado", "CO");
            states.Add("Connecticut", "CT");
            states.Add("Delaware", "DE");
            states.Add("District of Columbia", "DC");
            states.Add("Florida", "FL");
            states.Add("Georgia", "GA");
            states.Add("Hawaii", "HI");
            states.Add("Idaho", "ID");
            states.Add("Illinois", "IL");
            states.Add("Indiana", "IN");
            states.Add("Iowa", "IA");
            states.Add("Kansas", "KS");
            states.Add("Kentucky", "KY");
            states.Add("Louisiana", "LA");
            states.Add("Maine", "ME");
            states.Add("Maryland", "MD");
            states.Add("Massachusetts", "MA");
            states.Add("Michigan", "MI");
            states.Add("Minnesota", "MN");
            states.Add("Mississippi", "MS");
            states.Add("Missouri", "MO");
            states.Add("Montana", "MT");
            states.Add("Nebraska", "NE");
            states.Add("Nevada", "NV");
            states.Add("New Hampshire", "NH");
            states.Add("New Jersey", "NJ");
            states.Add("New Mexico", "NM");
            states.Add("New York", "NY");
            states.Add("North Carolina", "NC");
            states.Add("North Dakota", "ND");
            states.Add("Ohio", "OH");
            states.Add("Oklahoma", "OK");
            states.Add("Oregon", "OR");
            states.Add("Pennsylvania", "PA");
            states.Add("Rhode Island", "RI");
            states.Add("South Carolina", "SC");
            states.Add("South Dakota", "SD");
            states.Add("Tennessee", "TN");
            states.Add("Texas", "TX");
            states.Add("Utah", "UT");
            states.Add("Vermont", "VT");
            states.Add("Virginia", "VA");
            states.Add("Washington", "WA");
            states.Add("West Virginia", "WV");
            states.Add("Wisconsin", "WI");
            states.Add("Wyoming", "WY");
            states.Add("Armed Forces Americas", "AA");
            states.Add("Armed Forces Europe", "AE");
            states.Add("Armed Forces Pacific", "AP");
        }

        /// <summary>
        /// Load the province list
        /// </summary>
        private static void LoadProvinces()
        {
            provinces.Add("Alberta", "AB");
            provinces.Add("British Columbia", "BC");
            provinces.Add("Manitoba", "MB");
            provinces.Add("New Brunswick", "NB");
            provinces.Add("Newfoundland", "NF");
            provinces.Add("Northwest Territories", "NT");
            provinces.Add("Nova Scotia", "NS");
            provinces.Add("Nunavut", "NU");
            provinces.Add("Ontario", "ON");
            provinces.Add("Prince Edward Island", "PE");
            provinces.Add("Quebec", "QC");
            provinces.Add("Saskatchewan", "SK");
            provinces.Add("Yukon", "YT");
        }


        /// <summary>
        /// Load the list of countries without states/provinces
        /// </summary>
        private static void LoadCountriesWithNoStateProvinces()
        {
            countriesWithNoStateProvinces.Add("Great Britain", "GB");
        }

        #endregion
    }
}
