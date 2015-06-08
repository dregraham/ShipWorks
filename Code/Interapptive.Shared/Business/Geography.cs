using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using Interapptive.Shared.Properties;
using System.Collections;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Maintains countries, states, and provinces
    /// </summary>
    public static class Geography
    {
        static SortedList<string, string> countries = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        static SortedList<string, string> states = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        static SortedList<string, string> provinces = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);
        static SortedList<string, string> countriesWithNoStateProvinces = new SortedList<string, string>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Static contructor
        /// </summary>
        static Geography()
        {
            LoadCountries();
            LoadStates();
            LoadProvinces();
            LoadCountriesWithNoStateProvinces();
        }

        /// <summary>
        /// Returns a readonly list of Country names
        /// </summary>
        public static IList<string> Countries
        {
            get { return new List<string>(countries.Keys); }
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
        /// Indicates if the given foreign country code is a US international territory
        /// </summary>
        private static bool IsUSInternationalTerritory(string countryCode)
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

        /// <summary>
        /// Get the icon image of the country flag. Null if one is not present for the country.
        /// </summary>
        public static Image GetCountryFlag(string code)
        {
            #region Big Switch

            switch (code)
            {
                case "AL": return Flags.albania;
                case "DZ": return Flags.algeria;
                case "AS": return Flags.american_samoa;
                case "AD": return Flags.andorra;
                case "AO": return Flags.angola;
                case "AI": return Flags.anguilla;
                case "AG": return Flags.antigua_and_barbuda;
                case "AR": return Flags.argentina;
                case "AM": return Flags.armenia;
                case "AW": return Flags.aruba;
                case "AU": return Flags.australia;
                case "AT": return Flags.austria;
                case "AZ": return Flags.azerbaijan;
                case "AP": return null; // Azores
                case "BS": return Flags.bahamas;
                case "BH": return Flags.bahrain;
                case "BD": return Flags.bangladesh;
                case "BB": return Flags.barbados;
                case "BY": return Flags.belarus;
                case "BE": return Flags.belgium;
                case "BZ": return Flags.belize;
                case "BJ": return Flags.benin;
                case "BM": return Flags.bermuda;
                case "BO": return Flags.bolivia;
                case "BL": return null; // Bonaire
                case "BA": return Flags.bosnia_and_herzegovina;
                case "BW": return Flags.botswana;
                case "BR": return Flags.brazil;
                case "BN": return Flags.brunei;
                case "BG": return Flags.bulgaria;
                case "BF": return Flags.burkina_faso;
                case "BI": return Flags.burundi;
                case "KH": return Flags.cambodia;
                case "CM": return Flags.cameroon;
                case "CA": return Flags.canada;
                case "IC": return null; // Canary Islands
                case "CV": return Flags.cape_verde;
                case "KY": return Flags.cayman_islands;
                case "CF": return Flags.central_african_republic;
                case "TD": return Flags.chad;
                case "CL": return Flags.chile;
                case "CN": return Flags.china;
                case "CX": return null; // Christmas Island
                case "CC": return null; // Cocos (Keeling) Islands
                case "CO": return Flags.colombia;
                case "KM": return Flags.comoros;
                case "CG": return Flags.congo_republic;
                case "CD": return Flags.congo_democratic_republic;
                case "CK": return Flags.cook_islands;
                case "CR": return Flags.costa_rica;
                case "CI": return Flags.cote_divoire;
                case "HR": return Flags.croatia;
                case "CU": return Flags.cuba;
                case "CW": return null; // Curacao
                case "CY": return Flags.cyprus;
                case "CZ": return Flags.czech_republic;
                case "DK": return Flags.denmark;
                case "DJ": return Flags.djibouti;
                case "DM": return Flags.dominica;
                case "DO": return Flags.dominican_republic;
                case "TP": return Flags.east_timor;
                case "EC": return Flags.equador;
                case "EG": return Flags.egypt;
                case "SV": return Flags.el_salvador;
                case "GQ": return Flags.equatorial_guinea;
                case "ER": return Flags.eritrea;
                case "EE": return Flags.estonia;
                case "ET": return Flags.ethiopia;
                case "FO": return Flags.faroe_islands;
                case "FK": return Flags.falkland_islands;
                case "FJ": return Flags.fiji;
                case "FI": return Flags.finland;
                case "FR": return Flags.france;
                case "GF": return null; // French Guiana
                case "PF": return Flags.french_polynesia;
                case "TF": return Flags.france;
                case "GA": return Flags.gabon;
                case "GM": return Flags.gambia;
                case "GE": return Flags.georgia;
                case "DE": return Flags.germany;
                case "GH": return Flags.ghana;
                case "GI": return Flags.gibraltar;
                case "GB": return Flags.great_britain;
                case "GR": return Flags.greece;
                case "GL": return Flags.greenland;
                case "GD": return Flags.grenada;
                case "GP": return null; // Guadeloupe
                case "GU": return Flags.guam;
                case "GT": return Flags.guatemala;
                case "GG": return Flags.guernsey;
                case "GN": return Flags.guinea;
                case "GW": return Flags.guinea_bissau;
                case "GY": return Flags.guyana;
                case "HT": return Flags.haiti;
                case "HM": return null; // Heard And Mc Donald Islands
                case "HN": return Flags.honduras;
                case "HK": return Flags.hong_kong;
                case "HU": return Flags.hungary;
                case "IS": return Flags.iceland;
                case "IN": return Flags.india;
                case "ID": return Flags.indonesia;
                case "IE": return Flags.ireland;
                case "IL": return Flags.israel;
                case "IT": return Flags.italy;
                case "JM": return Flags.jamaica;
                case "JP": return Flags.japan;
                case "JE": return Flags.jersey;
                case "JO": return Flags.jordan;
                case "KZ": return Flags.kazakhstan;
                case "KE": return Flags.kenya;
                case "KI": return Flags.kiribati;
                case "KO": return null; // Kosrae
                case "KW": return Flags.kuwait;
                case "KG": return Flags.kyrgyzstan;
                case "LA": return Flags.laos;
                case "LV": return Flags.latvia;
                case "LB": return Flags.lebanon;
                case "LS": return Flags.lesotho;
                case "LR": return Flags.liberia;
                case "LY": return Flags.libya;
                case "LI": return Flags.liechtenstein;
                case "LT": return Flags.lithuania;
                case "LU": return Flags.luxembourg;
                case "MO": return Flags.macau;
                case "MK": return Flags.macedonia;
                case "MG": return Flags.madagascar;
                case "ME": return null; // Madeira
                case "MW": return Flags.malawi;
                case "MY": return Flags.malaysia;
                case "MV": return Flags.maledives;
                case "ML": return Flags.mali;
                case "MT": return Flags.malta;
                case "MH": return Flags.marshall_islands;
                case "MQ": return Flags.martinique;
                case "MR": return Flags.mauretania;
                case "MU": return Flags.mauritius;
                case "YT": return null; // Mayotte
                case "MX": return Flags.mexico;
                case "FM": return Flags.micronesia;
                case "MD": return Flags.moldova;
                case "MC": return Flags.monaco;
                case "MN": return Flags.mongolia;
                case "MS": return Flags.montserrat;
                case "MA": return Flags.morocco;
                case "MZ": return Flags.mozambique;
                case "MM": return Flags.burma;
                case "NA": return Flags.namibia;
                case "NR": return Flags.nauru;
                case "NP": return Flags.nepal;
                case "NL": return Flags.netherlands;
                case "AN": return Flags.netherlands_antilles;
                case "NC": return Flags.france;
                case "NZ": return Flags.new_zealand;
                case "NI": return Flags.nicaragua;
                case "NE": return Flags.niger;
                case "NG": return Flags.nigeria;
                case "NU": return Flags.niue;
                case "NF": return Flags.norfolk_island;
                case "KP": return Flags.north_korea;
                case "NB": return Flags.great_britain;
                case "MP": return Flags.northern_mariana_islands;
                case "NO": return Flags.norway;
                case "OM": return Flags.oman;
                case "PK": return Flags.pakistan;
                case "PW": return Flags.palau;
                case "PA": return Flags.panama;
                case "PG": return Flags.papua_new_guinea;
                case "PY": return Flags.paraquay;
                case "PE": return Flags.peru;
                case "PH": return Flags.philippines;
                case "PN": return Flags.pitcairn_islands;
                case "PL": return Flags.poland;
                case "PO": return null; // Ponape
                case "PT": return Flags.portugal;
                case "PR": return Flags.puerto_rico;
                case "QA": return Flags.qatar;
                case "RE": return null; // Reunion
                case "RO": return Flags.romania;
                case "RT": return Flags.northern_mariana_islands;
                case "RU": return Flags.russia;
                case "RW": return Flags.rwanda;
                case "NT": return Flags.france;
                case "SW": return Flags.saint_kitts_and_nevis;
                case "VI": return Flags.virgin_islands;
                case "EU": return null; // Saint Eustatius
                case "SH": return Flags.saint_helena;
                case "UV": return Flags.virgin_islands;
                case "KN": return Flags.saint_kitts_and_nevis;
                case "LC": return Flags.saint_lucia;
                case "MB": return null; // Saint Maaretn
                case "TB": return null; // Saint Martin
                case "SX": return null; // Sint Maarten
                case "PM": return Flags.saint_pierre_and_miquelon;
                case "VL": return null; // Saint Thomas
                case "VC": return Flags.saint_vincent_and_the_grenadines;
                case "SP": return Flags.northern_mariana_islands;
                case "WS": return Flags.samoa;
                case "SM": return Flags.san_marino;
                case "ST": return Flags.sao_tome_and_principe;
                case "SA": return Flags.saudi_arabia;
                case "SF": return Flags.scotland;
                case "SN": return Flags.senegal;
                case "CS": return Flags.serbia_montenegro;
                case "SC": return Flags.seychelles;
                case "SL": return Flags.sierra_leone;
                case "SG": return Flags.singapore;
                case "SK": return Flags.slovakia;
                case "SI": return Flags.slovenia;
                case "SB": return Flags.solomon_islands;
                case "SO": return Flags.somalia;
                case "ZA": return Flags.south_africa;
                case "GS": return Flags.south_georgia;
                case "KR": return Flags.south_korea;
                case "ES": return Flags.spain;
                case "LK": return Flags.sri_lanka;
                case "SD": return Flags.sudan;
                case "SR": return Flags.suriname;
                case "SJ": return null; // Svalbard and Jan Mayen Islands
                case "SZ": return Flags.swaziland;
                case "SE": return Flags.sweden;
                case "CH": return Flags.switzerland;
                case "SY": return Flags.syria;
                case "TA": return Flags.austria;
                case "TW": return Flags.taiwan;
                case "TJ": return Flags.tajikistan;
                case "TZ": return Flags.tanzania;
                case "TH": return Flags.thailand;
                case "TI": return Flags.northern_mariana_islands;
                case "TG": return Flags.togo;
                case "TK": return null; // Tokelau
                case "TO": return Flags.tonga;
                case "TL": return Flags.british_virgin_islands;
                case "TT": return Flags.trinidad_and_tobago;
                case "TU": return null; // Truk
                case "TN": return Flags.tunisia;
                case "TR": return Flags.turkey;
                case "TM": return Flags.turkmenistan;
                case "TC": return Flags.turks_and_caicos_islands;
                case "TV": return Flags.tuvalu;
                case "UG": return Flags.uganda;
                case "UA": return Flags.ukraine;
                case "UI": return Flags.saint_vincent_and_the_grenadines;
                case "AE": return Flags.united_arab_emirates;
                case "UK": return Flags.great_britain;
                case "US": return Flags.usa;
                case "UY": return Flags.uruquay;
                case "UM": return Flags.usa;
                case "UZ": return Flags.uzbekistan;
                case "VU": return Flags.vanuatu;
                case "VA": return Flags.vatican_city;
                case "VE": return Flags.venezuela;
                case "VN": return Flags.vietnam;
                case "VR": return Flags.british_virgin_islands;
                case "VG": return Flags.british_virgin_islands;
                case "WF": return Flags.wallis_and_futuna;
                case "YE": return Flags.yemen;
                case "ZR": return null; // Zaire
                case "ZM": return Flags.zambia;
                case "ZW": return Flags.zimbabwe;
        }

            #endregion

            return null;
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
        /// Load the country list
        /// </summary>
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
