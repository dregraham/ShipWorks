using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Interapptive.Shared.Properties;

namespace Interapptive.Shared.Business.Geography
{
    /// <summary>
    /// Maintains the list of country and flag associations
    /// </summary>
    internal static class CountryFlags
    {
        /// <summary>
        /// Get the icon image of the country flag. Null if one is not present for the country.
        /// </summary>
        public static Image GetCountryFlag(string code)
        {
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

            return null;
        }
    }
}
