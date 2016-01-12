using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using System.Collections.ObjectModel;
using System.Linq;
using Interapptive.Shared.Collections;

namespace Interapptive.Shared.Business.Geography
{
    /// <summary>
    /// Maintains countries
    /// </summary>
    public static class CountryList
    {
        static SortedList<string, string> countries = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Static contructor
        /// </summary>
        static CountryList()
        {
            LoadCountries();
        }

        /// <summary>
        /// Returns a readonly list of Country names
        /// </summary>
        public static IList<string> CountryNames
        {
            get { return new List<string>(countries.Keys); }
        }

        /// <summary>
        /// Returns a readonly list of countries
        /// </summary>
        public static IEnumerable<KeyValuePair<string, string>> Countries => countries.ToReadOnly();

        /// <summary>
        /// Get the code of the country with the given name.  If not found, the original name is returned.
        /// </summary>
        public static string GetCountryCode(string name)
        {
            string code;
            if (countries.TryGetValue(name, out code))
            {
                return code;
            }

            // Saint Croix is just US VI, but not in our list of names.  But we want to be able support carts that send us that name
            if (name == "Saint Croix")
            {
                return "VI";
            }

            // Some carts send down variations of the US country name
            if (String.Compare("united states", name, StringComparison.OrdinalIgnoreCase) == 0 || String.Compare("usa", name, StringComparison.OrdinalIgnoreCase) == 0 || String.Compare("united states of america", name, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return "US";
            }

            // Check for lowercase codes
            if (name.Length <= 2)
            {
                return name.ToUpper();
            }

            return name;
        }

        /// <summary>
        /// Get the name of the country based on the given country code.  If not found, the original code is returned.
        /// </summary>
        public static string GetCountryName(string code)
        {
            if (code == null)
            {
                return code;
            }

            int index = countries.IndexOfValue(code);
            if (index != -1)
            {
                return countries.Keys[index];
            }

            // Serbia-Montenegro split, and CS became Servia which is "RS"
            if (code.ToLower() == "cs")
            {
                return GetCountryName("RS");
            }

            return code;
        }

        /// <summary>
        /// Indicates if the given foreign country code is a US international territory
        /// </summary>
        public static bool IsUSInternationalTerritory(this IAddressAdapter address)
        {
            MethodConditions.EnsureArgumentIsNotNull(address, "address");

            return IsUSInternationalTerritory(address.CountryCode) || 
                (string.Equals(address.CountryCode, "US", StringComparison.OrdinalIgnoreCase) && IsUSInternationalTerritory(address.StateProvCode));
        }

        /// <summary>
        /// Indicates if the given foreign country code is a specific US international territory
        /// </summary>
        public static bool IsUSInternationalTerritory(this IAddressAdapter address, string territory)
        {
            MethodConditions.EnsureArgumentIsNotNull(address, "address");

            return address.CountryCode.Equals(territory, StringComparison.OrdinalIgnoreCase) ||
                (address.CountryCode.Equals("US", StringComparison.OrdinalIgnoreCase) && address.StateProvCode.Equals(territory, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Indicates if the given foreign country code is a US international territory
        /// </summary>
        public static bool IsUSInternationalTerritory(string countryCode)
        {
            return

                // American Samoa
                countryCode == "AS" ||

                // Federated States of Micronesia
                countryCode == "FM" ||

                // Guam
                countryCode == "GU" ||

                // Marshall Islands
                countryCode == "MH" ||

                // Northern Mariana Islands
                countryCode == "MP" ||

                // Palau
                countryCode == "PW" ||

                // Puerto Rico
                countryCode == "PR" ||

                // Virgin Islands
                countryCode == "VI" ||
                countryCode == "VL" ||
                countryCode == "UV";
        }

        #region Loading

        /// <summary>
        /// Load the country list
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void LoadCountries()
        {
            countries.Add("Albania", "AL");
            countries.Add("Algeria", "DZ");
            countries.Add("American Samoa", "AS");
            countries.Add("Andorra", "AD");
            countries.Add("Angola", "AO");
            countries.Add("Anguilla", "AI");
            countries.Add("Antigua and Barbuda", "AG");
            countries.Add("Argentina", "AR");
            countries.Add("Armenia", "AM");
            countries.Add("Aruba", "AW");
            countries.Add("Australia", "AU");
            countries.Add("Austria", "AT");
            countries.Add("Azerbaijan", "AZ");
            countries.Add("Azores", "AP");
            countries.Add("Bahamas", "BS");
            countries.Add("Bahrain", "BH");
            countries.Add("Bangladesh", "BD");
            countries.Add("Barbados", "BB");
            countries.Add("Belarus", "BY");
            countries.Add("Belgium", "BE");
            countries.Add("Belize", "BZ");
            countries.Add("Benin", "BJ");
            countries.Add("Bermuda", "BM");
            countries.Add("Bhutan", "BT");
            countries.Add("Bolivia", "BO");
            countries.Add("Bonaire", "BL");
            countries.Add("Bosnia", "BA");
            countries.Add("Botswana", "BW");
            countries.Add("Brazil", "BR");
            countries.Add("Brunei Darussalam", "BN");
            countries.Add("Bulgaria", "BG");
            countries.Add("Burkina Faso", "BF");
            countries.Add("Burundi", "BI");
            countries.Add("Cambodia", "KH");
            countries.Add("Cameroon", "CM");
            countries.Add("Canada", "CA");
            countries.Add("Canary Islands", "IC");
            countries.Add("Cape Verde Island", "CV");
            countries.Add("Cayman Islands", "KY");
            countries.Add("Central African Republic", "CF");
            countries.Add("Chad", "TD");
            countries.Add("Chile", "CL");
            countries.Add("China", "CN");
            countries.Add("Christmas Island", "CX");
            countries.Add("Cocos (Keeling) Islands", "CC");
            countries.Add("Colombia", "CO");
            countries.Add("Comoros", "KM");
            countries.Add("Congo", "CG");
            countries.Add("Congo, Democratic Repulic of", "CD");
            countries.Add("Cook Islands", "CK");
            countries.Add("Costa Rica", "CR");
            countries.Add("Cote d'Ivoire", "CI");
            countries.Add("Croatia", "HR");
            countries.Add("Cuba", "CU");
            countries.Add("Curacao", "CW");
            countries.Add("Cyprus", "CY");
            countries.Add("Czech Republic", "CZ");
            countries.Add("Denmark", "DK");
            countries.Add("Djibouti", "DJ");
            countries.Add("Dominica", "DM");
            countries.Add("Dominican Republic", "DO");
            countries.Add("East Timor", "TP");
            countries.Add("Ecuador", "EC");
            countries.Add("Egypt", "EG");
            countries.Add("El Salvador", "SV");
            countries.Add("Equatorial Guinea", "GQ");
            countries.Add("Eritrea", "ER");
            countries.Add("Estonia", "EE");
            countries.Add("Ethiopia", "ET");
            countries.Add("Falkland Islands (Malvinas)", "FK");
            countries.Add("Faroe Islands", "FO");
            countries.Add("Fiji", "FJ");
            countries.Add("Finland", "FI");
            countries.Add("France", "FR");
            countries.Add("French Guiana", "GF");
            countries.Add("French Polynesia", "PF");
            countries.Add("French Southern Territories", "TF");
            countries.Add("Gabon", "GA");
            countries.Add("Gambia", "GM");
            countries.Add("Georgia", "GE");
            countries.Add("Germany", "DE");
            countries.Add("Ghana", "GH");
            countries.Add("Gibraltar", "GI");
            countries.Add("Great Britain", "GB");
            countries.Add("Greece", "GR");
            countries.Add("Greenland", "GL");
            countries.Add("Grenada", "GD");
            countries.Add("Guadeloupe", "GP");
            countries.Add("Guam", "GU");
            countries.Add("Guatemala", "GT");
            countries.Add("Guernsey", "GG");
            countries.Add("Guinea", "GN");
            countries.Add("Guinea-Bissau", "GW");
            countries.Add("Guyana", "GY");
            countries.Add("Haiti", "HT");
            countries.Add("Heard And Mc Donald Islands", "HM");
            countries.Add("Honduras", "HN");
            countries.Add("Hong Kong", "HK");
            countries.Add("Hungary", "HU");
            countries.Add("Iceland", "IS");
            countries.Add("India", "IN");
            countries.Add("Indonesia", "ID");
            countries.Add("Iraq", "IQ");
            countries.Add("Ireland", "IE");
            countries.Add("Israel", "IL");
            countries.Add("Italy", "IT");
            countries.Add("Jamaica", "JM");
            countries.Add("Japan", "JP");
            countries.Add("Jersey", "JE");
            countries.Add("Jordan", "JO");
            countries.Add("Kazakhstan", "KZ");
            countries.Add("Kenya", "KE");
            countries.Add("Kiribati", "KI");
            countries.Add("Kosrae", "KO");
            countries.Add("North Korea", "KP");
            countries.Add("South Korea", "KR");
            countries.Add("Kuwait", "KW");
            countries.Add("Kyrgyzstan", "KG");
            countries.Add("Laos", "LA");
            countries.Add("Lebanon", "LB");
            countries.Add("Latvia", "LV");
            countries.Add("Lesotho", "LS");
            countries.Add("Liberia", "LR");
            countries.Add("Libyan Arab Jamahiriya", "LY");
            countries.Add("Liechtenstein", "LI");
            countries.Add("Lithuania", "LT");
            countries.Add("Luxembourg", "LU");
            countries.Add("Macau", "MO");
            countries.Add("Macedonia", "MK");
            countries.Add("Madagascar", "MG");
            countries.Add("Malawi", "MW");
            countries.Add("Malaysia", "MY");
            countries.Add("Maldives", "MV");
            countries.Add("Mali", "ML");
            countries.Add("Malta", "MT");
            countries.Add("Marshall Islands", "MH");
            countries.Add("Martinique", "MQ");
            countries.Add("Mauritania", "MR");
            countries.Add("Mauritius", "MU");
            countries.Add("Mayotte", "YT");
            countries.Add("Mexico", "MX");
            countries.Add("Micronesia", "FM");
            countries.Add("Moldova", "MD");
            countries.Add("Monaco", "MC");
            countries.Add("Mongolia", "MN");
            countries.Add("Montenegro", "ME");
            countries.Add("Montserrat", "MS");
            countries.Add("Morocco", "MA");
            countries.Add("Mozambique", "MZ");
            countries.Add("Myanmar", "MM");
            countries.Add("Namibia", "NA");
            countries.Add("Nauru", "NR");
            countries.Add("Nepal", "NP");
            countries.Add("Netherlands", "NL");
            countries.Add("Netherlands Antilles", "AN");
            countries.Add("Neutral Zone", "NT");
            countries.Add("New Caledonia", "NC");
            countries.Add("New Zealand", "NZ");
            countries.Add("Nicaragua", "NI");
            countries.Add("Niger", "NE");
            countries.Add("Nigeria", "NG");
            countries.Add("Niue", "NU");
            countries.Add("Norfolk Island", "NF");
            countries.Add("Northern Ireland", "NB");
            countries.Add("Northern Mariana Islands", "MP");
            countries.Add("Norway", "NO");
            countries.Add("Oman", "OM");
            countries.Add("Pakistan", "PK");
            countries.Add("Palau", "PW");
            countries.Add("Panama", "PA");
            countries.Add("Papua New Guinea", "PG");
            countries.Add("Paraguay", "PY");
            countries.Add("Peru", "PE");
            countries.Add("Philippines", "PH");
            countries.Add("Pitcairn", "PN");
            countries.Add("Poland", "PL");
            countries.Add("Ponape", "PO");
            countries.Add("Portugal", "PT");
            countries.Add("Puerto Rico", "PR");
            countries.Add("Qatar", "QA");
            countries.Add("Reunion", "RE");
            countries.Add("Romania", "RO");
            countries.Add("Rota", "RT");
            countries.Add("Russia", "RU");
            countries.Add("Rwanda", "RW");
            countries.Add("Saint Helena", "SH");
            countries.Add("Saint Kitts and Nevis", "KN");
            countries.Add("Saint Lucia", "LC");
            countries.Add("Saint Pierre and Miquelon", "PM");
            countries.Add("Saint Vincent and The Grenadines", "VC");
            countries.Add("Saipan", "SP");
            countries.Add("Samoa", "WS");
            countries.Add("San Marino", "SM");
            countries.Add("Sao Tome and Principe", "ST");
            countries.Add("Saudi Arabia", "SA");
            countries.Add("Scotland", "SF");
            countries.Add("Senegal", "SN");
            countries.Add("Seychelles", "SC");
            countries.Add("Sierra Leone", "SL");
            countries.Add("Singapore", "SG");
            countries.Add("Slovak Republic", "SK");
            countries.Add("Slovenia", "SI");
            countries.Add("Solomon Islands", "SB");
            countries.Add("Somalia", "SO");
            countries.Add("South Africa", "ZA");
            countries.Add("South Georgia and South Sandwich Islands", "GS");
            countries.Add("Spain", "ES");
            countries.Add("Sri Lanka", "LK");
            countries.Add("Saint Barthelemy", "NT");
            countries.Add("Saint Christopher", "SW");
            countries.Add("Saint Eustatius", "EU");
            countries.Add("Saint John", "UV");
            countries.Add("Saint Maaretn", "MB");
            countries.Add("Saint Martin", "TB");
            countries.Add("Sint Maarten", "SX");
            countries.Add("Saint Thomas", "VL");
            countries.Add("Sudan", "SD");
            countries.Add("Suriname", "SR");
            countries.Add("Svalbard and Jan Mayen Islands", "SJ");
            countries.Add("Swaziland", "SZ");
            countries.Add("Sweden", "SE");
            countries.Add("Switzerland", "CH");
            countries.Add("Syrian Arab Republic", "SY");
            countries.Add("Tahiti", "TA");
            countries.Add("Taiwan", "TW");
            countries.Add("Tajikistan", "TJ");
            countries.Add("Tanzania", "TZ");
            countries.Add("Thailand", "TH");
            countries.Add("Tinian", "TI");
            countries.Add("Togo", "TG");
            countries.Add("Tortola", "TL");
            countries.Add("Tokelau", "TK");
            countries.Add("Tonga", "TO");
            countries.Add("Trinidad and Tobago", "TT");
            countries.Add("Truk", "TU");
            countries.Add("Tunisia", "TN");
            countries.Add("Turkey", "TR");
            countries.Add("Turkmenistan", "TM");
            countries.Add("Turks Islands and Caicos Islands", "TC");
            countries.Add("Tuvalu", "TV");
            countries.Add("Uganda", "UG");
            countries.Add("Ukraine", "UA");
            countries.Add("Union Island", "UI");
            countries.Add("United Arab Emirates", "AE");
            countries.Add("United Kingdom", "UK");
            countries.Add("United States", "US");
            countries.Add("US Minor Outlying Islands", "UM");
            countries.Add("Uruguay", "UY");
            countries.Add("Uzbekistan", "UZ");
            countries.Add("Vanuatu", "VU");
            countries.Add("Vatican City State", "VA");
            countries.Add("Venezuela", "VE");
            countries.Add("Vietnam", "VN");
            countries.Add("Virgin Gorda", "VR");
            countries.Add("Virgin Islands (British)", "VG");
            countries.Add("Virgin Islands (US)", "VI");
            countries.Add("Wallis and Futuna Islands", "WF");
            countries.Add("Western Samoa", "WS");
            countries.Add("Yemen", "YE");
            countries.Add("Serbia", "RS");
            countries.Add("Zaire", "ZR");
            countries.Add("Zambia", "ZM");
            countries.Add("Zimbabwe", "ZW");
        }

        #endregion
    }
}
