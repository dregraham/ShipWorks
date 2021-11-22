using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Enums
{
    /// <summary>
    /// Unit Type
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public enum CustomsTinIssuingAuthority
    {
        [Description("Albania")]
        [EnumMember(Value = "AL")]
        AL = 0,

        [Description("Algeria")]
        [EnumMember(Value = "DZ")]
        DZ = 1,

        [Description("American Samoa")]
        [EnumMember(Value = "AS")]
        AS = 2,

        [Description("Andorra")]
        [EnumMember(Value = "AD")]
        AD = 3,

        [Description("Angola")]
        [EnumMember(Value = "AO")]
        AO = 4,

        [Description("Anguilla")]
        [EnumMember(Value = "AI")]
        AI = 5,

        [Description("")]
        [EnumMember(Value = "undefined")]
        undefined = 6,

        [Description("Antigua and Barbuda")]
        [EnumMember(Value = "AG")]
        AG = 7,

        [Description("Argentina")]
        [EnumMember(Value = "AR")]
        AR = 8,

        [Description("Armenia")]
        [EnumMember(Value = "AM")]
        AM = 9,

        [Description("Aruba")]
        [EnumMember(Value = "AW")]
        AW = 10,

        [Description("Australia")]
        [EnumMember(Value = "AU")]
        AU = 11,

        [Description("Austria")]
        [EnumMember(Value = "AT")]
        AT = 12,

        [Description("Azerbaijan")]
        [EnumMember(Value = "AZ")]
        AZ = 13,

        [Description("Azores")]
        [EnumMember(Value = "AP")]
        AP = 14,

        [Description("Bahamas")]
        [EnumMember(Value = "BS")]
        BS = 15,

        [Description("Bahrain")]
        [EnumMember(Value = "BH")]
        BH = 16,

        [Description("Bangladesh")]
        [EnumMember(Value = "BD")]
        BD = 17,

        [Description("Barbados")]
        [EnumMember(Value = "BB")]
        BB = 18,

        [Description("Belarus")]
        [EnumMember(Value = "BY")]
        BY = 19,

        [Description("Belgium")]
        [EnumMember(Value = "BE")]
        BE = 20,

        [Description("Belize")]
        [EnumMember(Value = "BZ")]
        BZ = 21,

        [Description("Benin")]
        [EnumMember(Value = "BJ")]
        BJ = 22,

        [Description("Bermuda")]
        [EnumMember(Value = "BM")]
        BM = 23,

        [Description("Bhutan")]
        [EnumMember(Value = "BT")]
        BT = 24,

        [Description("Bolivia")]
        [EnumMember(Value = "BO")]
        BO = 25,

        [Description("Bonaire")]
        [EnumMember(Value = "BQ")]
        BQ = 26,

        [Description("Bosnia")]
        [EnumMember(Value = "BA")]
        BA = 27,

        [Description("Botswana")]
        [EnumMember(Value = "BW")]
        BW = 28,

        [Description("Brazil")]
        [EnumMember(Value = "BR")]
        BR = 29,

        [Description("Brunei Darussalam")]
        [EnumMember(Value = "BN")]
        BN = 30,

        [Description("Bulgaria")]
        [EnumMember(Value = "BG")]
        BG = 31,

        [Description("Burkina Faso")]
        [EnumMember(Value = "BF")]
        BF = 32,

        [Description("Burundi")]
        [EnumMember(Value = "BI")]
        BI = 33,

        [Description("Cambodia")]
        [EnumMember(Value = "KH")]
        KH = 34,

        [Description("Cameroon")]
        [EnumMember(Value = "CM")]
        CM = 35,

        [Description("Canada")]
        [EnumMember(Value = "CA")]
        CA = 36,

        [Description("Canary Islands")]
        [EnumMember(Value = "IC")]
        IC = 37,

        [Description("Cape Verde Island")]
        [EnumMember(Value = "CV")]
        CV = 38,

        [Description("Cayman Islands")]
        [EnumMember(Value = "KY")]
        KY = 39,

        [Description("Central African Republic")]
        [EnumMember(Value = "CF")]
        CF = 40,

        [Description("Chad")]
        [EnumMember(Value = "TD")]
        TD = 41,

        [Description("Chile")]
        [EnumMember(Value = "CL")]
        CL = 42,

        [Description("China")]
        [EnumMember(Value = "CN")]
        CN = 43,

        [Description("Christmas Island")]
        [EnumMember(Value = "CX")]
        CX = 44,

        [Description("Cocos (Keeling) Islands")]
        [EnumMember(Value = "CC")]
        CC = 45,

        [Description("Colombia")]
        [EnumMember(Value = "CO")]
        CO = 46,

        [Description("Comoros")]
        [EnumMember(Value = "KM")]
        KM = 47,

        [Description("Congo")]
        [EnumMember(Value = "CG")]
        CG = 48,

        [Description("Cook Islands")]
        [EnumMember(Value = "CK")]
        CK = 50,

        [Description("Costa Rica")]
        [EnumMember(Value = "CR")]
        CR = 51,

        [Description("Cote d'Ivoire")]
        [EnumMember(Value = "CI")]
        CI = 52,

        [Description("Croatia")]
        [EnumMember(Value = "HR")]
        HR = 53,

        [Description("Cuba")]
        [EnumMember(Value = "CU")]
        CU = 54,

        [Description("Curacao")]
        [EnumMember(Value = "CW")]
        CW = 55,

        [Description("Cyprus")]
        [EnumMember(Value = "CY")]
        CY = 56,

        [Description("Czech Republic")]
        [EnumMember(Value = "CZ")]
        CZ = 57,

        [Description("Denmark")]
        [EnumMember(Value = "DK")]
        DK = 58,

        [Description("Djibouti")]
        [EnumMember(Value = "DJ")]
        DJ = 59,

        [Description("Dominica")]
        [EnumMember(Value = "DM")]
        DM = 60,

        [Description("Dominican Republic")]
        [EnumMember(Value = "DO")]
        DO = 61,

        [Description("East Timor")]
        [EnumMember(Value = "TP")]
        TP = 62,

        [Description("Ecuador")]
        [EnumMember(Value = "EC")]
        EC = 63,

        [Description("Egypt")]
        [EnumMember(Value = "EG")]
        EG = 64,

        [Description("El Salvador")]
        [EnumMember(Value = "SV")]
        SV = 65,

        [Description("Equatorial Guinea")]
        [EnumMember(Value = "GQ")]
        GQ = 66,

        [Description("Eritrea")]
        [EnumMember(Value = "ER")]
        ER = 67,

        [Description("Estonia")]
        [EnumMember(Value = "EE")]
        EE = 68,

        [Description("Ethiopia")]
        [EnumMember(Value = "ET")]
        ET = 69,

        [Description("Falkland Islands (Malvinas)")]
        [EnumMember(Value = "FK")]
        FK = 70,

        [Description("Faroe Islands")]
        [EnumMember(Value = "FO")]
        FO = 71,

        [Description("Fiji")]
        [EnumMember(Value = "FJ")]
        FJ = 72,

        [Description("Finland")]
        [EnumMember(Value = "FI")]
        FI = 73,

        [Description("France")]
        [EnumMember(Value = "FR")]
        FR = 74,

        [Description("French Guiana")]
        [EnumMember(Value = "GF")]
        GF = 75,

        [Description("French Polynesia")]
        [EnumMember(Value = "PF")]
        PF = 76,

        [Description("French Southern Territories")]
        [EnumMember(Value = "TF")]
        TF = 77,

        [Description("Gabon")]
        [EnumMember(Value = "GA")]
        GA = 78,

        [Description("Gambia")]
        [EnumMember(Value = "GM")]
        GM = 79,

        [Description("Georgia")]
        [EnumMember(Value = "GE")]
        GE = 80,

        [Description("Germany")]
        [EnumMember(Value = "DE")]
        DE = 81,

        [Description("Ghana")]
        [EnumMember(Value = "GH")]
        GH = 82,

        [Description("Gibraltar")]
        [EnumMember(Value = "GI")]
        GI = 83,

        [Description("Great Britain")]
        [EnumMember(Value = "GB")]
        GB = 84,

        [Description("Greece")]
        [EnumMember(Value = "GR")]
        GR = 85,

        [Description("Greenland")]
        [EnumMember(Value = "GL")]
        GL = 86,

        [Description("Grenada")]
        [EnumMember(Value = "GD")]
        GD = 87,

        [Description("Guadeloupe")]
        [EnumMember(Value = "GP")]
        GP = 88,

        [Description("Guam")]
        [EnumMember(Value = "GU")]
        GU = 89,

        [Description("Guatemala")]
        [EnumMember(Value = "GT")]
        GT = 90,

        [Description("Guernsey")]
        [EnumMember(Value = "GG")]
        GG = 91,

        [Description("Guinea")]
        [EnumMember(Value = "GN")]
        GN = 92,

        [Description("Guinea-Bissau")]
        [EnumMember(Value = "GW")]
        GW = 93,

        [Description("Guyana")]
        [EnumMember(Value = "GY")]
        GY = 94,

        [Description("Haiti")]
        [EnumMember(Value = "HT")]
        HT = 95,

        [Description("Heard And Mc Donald Islands")]
        [EnumMember(Value = "HM")]
        HM = 96,

        [Description("Honduras")]
        [EnumMember(Value = "HN")]
        HN = 97,

        [Description("Hong Kong")]
        [EnumMember(Value = "HK")]
        HK = 98,

        [Description("Hungary")]
        [EnumMember(Value = "HU")]
        HU = 99,

        [Description("Iceland")]
        [EnumMember(Value = "IS")]
        IS = 100,

        [Description("India")]
        [EnumMember(Value = "IN")]
        IN = 101,

        [Description("Indonesia")]
        [EnumMember(Value = "ID")]
        ID = 102,

        [Description("Iraq")]
        [EnumMember(Value = "IQ")]
        IQ = 103,

        [Description("Ireland")]
        [EnumMember(Value = "IE")]
        IE = 104,

        [Description("Israel")]
        [EnumMember(Value = "IL")]
        IL = 105,

        [Description("Italy")]
        [EnumMember(Value = "IT")]
        IT = 106,

        [Description("Jamaica")]
        [EnumMember(Value = "JM")]
        JM = 107,

        [Description("Japan")]
        [EnumMember(Value = "JP")]
        JP = 108,

        [Description("Jersey")]
        [EnumMember(Value = "JE")]
        JE = 109,

        [Description("Jordan")]
        [EnumMember(Value = "JO")]
        JO = 110,

        [Description("Kazakhstan")]
        [EnumMember(Value = "KZ")]
        KZ = 111,

        [Description("Kenya")]
        [EnumMember(Value = "KE")]
        KE = 112,

        [Description("Kiribati")]
        [EnumMember(Value = "KI")]
        KI = 113,

        [Description("Kosrae")]
        [EnumMember(Value = "KO")]
        KO = 114,

        [Description("North Korea")]
        [EnumMember(Value = "KP")]
        KP = 115,

        [Description("South Korea")]
        [EnumMember(Value = "KR")]
        KR = 116,

        [Description("Kuwait")]
        [EnumMember(Value = "KW")]
        KW = 117,

        [Description("Kyrgyzstan")]
        [EnumMember(Value = "KG")]
        KG = 118,

        [Description("Laos")]
        [EnumMember(Value = "LA")]
        LA = 119,

        [Description("Lebanon")]
        [EnumMember(Value = "LB")]
        LB = 120,

        [Description("Latvia")]
        [EnumMember(Value = "LV")]
        LV = 121,

        [Description("Lesotho")]
        [EnumMember(Value = "LS")]
        LS = 122,

        [Description("Liberia")]
        [EnumMember(Value = "LR")]
        LR = 123,

        [Description("Libyan Arab Jamahiriya")]
        [EnumMember(Value = "LY")]
        LY = 124,

        [Description("Liechtenstein")]
        [EnumMember(Value = "LI")]
        LI = 125,

        [Description("Lithuania")]
        [EnumMember(Value = "LT")]
        LT = 126,

        [Description("Luxembourg")]
        [EnumMember(Value = "LU")]
        LU = 127,

        [Description("Macau")]
        [EnumMember(Value = "MO")]
        MO = 128,

        [Description("Macedonia")]
        [EnumMember(Value = "MK")]
        MK = 129,

        [Description("Madagascar")]
        [EnumMember(Value = "MG")]
        MG = 130,

        [Description("Malawi")]
        [EnumMember(Value = "MW")]
        MW = 131,

        [Description("Malaysia")]
        [EnumMember(Value = "MY")]
        MY = 132,

        [Description("Maldives")]
        [EnumMember(Value = "MV")]
        MV = 133,

        [Description("Mali")]
        [EnumMember(Value = "ML")]
        ML = 134,

        [Description("Malta")]
        [EnumMember(Value = "MT")]
        MT = 135,

        [Description("Marshall Islands")]
        [EnumMember(Value = "MH")]
        MH = 136,

        [Description("Martinique")]
        [EnumMember(Value = "MQ")]
        MQ = 137,

        [Description("Mauritania")]
        [EnumMember(Value = "MR")]
        MR = 138,

        [Description("Mauritius")]
        [EnumMember(Value = "MU")]
        MU = 139,

        [Description("Mayotte")]
        [EnumMember(Value = "YT")]
        YT = 140,

        [Description("Mexico")]
        [EnumMember(Value = "MX")]
        MX = 141,

        [Description("Micronesia")]
        [EnumMember(Value = "FM")]
        FM = 142,

        [Description("Moldova")]
        [EnumMember(Value = "MD")]
        MD = 143,

        [Description("Monaco")]
        [EnumMember(Value = "MC")]
        MC = 144,

        [Description("Mongolia")]
        [EnumMember(Value = "MN")]
        MN = 145,

        [Description("Montenegro")]
        [EnumMember(Value = "ME")]
        ME = 146,

        [Description("Montserrat")]
        [EnumMember(Value = "MS")]
        MS = 147,

        [Description("Morocco")]
        [EnumMember(Value = "MA")]
        MA = 148,

        [Description("Mozambique")]
        [EnumMember(Value = "MZ")]
        MZ = 149,

        [Description("Myanmar")]
        [EnumMember(Value = "MM")]
        MM = 150,

        [Description("Namibia")]
        [EnumMember(Value = "NA")]
        NA = 151,

        [Description("Nauru")]
        [EnumMember(Value = "NR")]
        NR = 152,

        [Description("Nepal")]
        [EnumMember(Value = "NP")]
        NP = 153,

        [Description("Netherlands")]
        [EnumMember(Value = "NL")]
        NL = 154,

        [Description("Netherlands Antilles")]
        [EnumMember(Value = "AN")]
        AN = 155,

        [Description("New Caledonia")]
        [EnumMember(Value = "NC")]
        NC = 156,

        [Description("New Zealand")]
        [EnumMember(Value = "NZ")]
        NZ = 157,

        [Description("Nicaragua")]
        [EnumMember(Value = "NI")]
        NI = 158,

        [Description("Niger")]
        [EnumMember(Value = "NE")]
        NE = 159,

        [Description("Nigeria")]
        [EnumMember(Value = "NG")]
        NG = 160,

        [Description("Niue")]
        [EnumMember(Value = "NU")]
        NU = 161,

        [Description("Norfolk Island")]
        [EnumMember(Value = "NF")]
        NF = 162,

        [Description("Northern Ireland")]
        [EnumMember(Value = "NB")]
        NB = 163,

        [Description("Northern Mariana Islands")]
        [EnumMember(Value = "MP")]
        MP = 164,

        [Description("Norway")]
        [EnumMember(Value = "NO")]
        NO = 165,

        [Description("Oman")]
        [EnumMember(Value = "OM")]
        OM = 166,

        [Description("Pakistan")]
        [EnumMember(Value = "PK")]
        PK = 167,

        [Description("Palau")]
        [EnumMember(Value = "PW")]
        PW = 168,

        [Description("Panama")]
        [EnumMember(Value = "PA")]
        PA = 169,

        [Description("Papua New Guinea")]
        [EnumMember(Value = "PG")]
        PG = 170,

        [Description("Paraguay")]
        [EnumMember(Value = "PY")]
        PY = 171,

        [Description("Peru")]
        [EnumMember(Value = "PE")]
        PE = 172,

        [Description("Philippines")]
        [EnumMember(Value = "PH")]
        PH = 173,

        [Description("Pitcairn")]
        [EnumMember(Value = "PN")]
        PN = 174,

        [Description("Poland")]
        [EnumMember(Value = "PL")]
        PL = 175,

        [Description("Ponape")]
        [EnumMember(Value = "PO")]
        PO = 176,

        [Description("Portugal")]
        [EnumMember(Value = "PT")]
        PT = 177,

        [Description("Puerto Rico")]
        [EnumMember(Value = "PR")]
        PR = 178,

        [Description("Qatar")]
        [EnumMember(Value = "QA")]
        QA = 179,

        [Description("Reunion")]
        [EnumMember(Value = "RE")]
        RE = 180,

        [Description("Romania")]
        [EnumMember(Value = "RO")]
        RO = 181,

        [Description("Rota")]
        [EnumMember(Value = "RT")]
        RT = 182,

        [Description("Russia")]
        [EnumMember(Value = "RU")]
        RU = 183,

        [Description("Rwanda")]
        [EnumMember(Value = "RW")]
        RW = 184,

        [Description("Saint Helena")]
        [EnumMember(Value = "SH")]
        SH = 185,

        [Description("Saint Kitts and Nevis")]
        [EnumMember(Value = "KN")]
        KN = 186,

        [Description("Saint Lucia")]
        [EnumMember(Value = "LC")]
        LC = 187,

        [Description("Saint Pierre and Miquelon")]
        [EnumMember(Value = "PM")]
        PM = 188,

        [Description("Saint Vincent and The Grenadines")]
        [EnumMember(Value = "VC")]
        VC = 189,

        [Description("Saipan")]
        [EnumMember(Value = "SP")]
        SP = 190,

        [Description("Samoa")]
        [EnumMember(Value = "WS")]
        WS = 191,

        [Description("San Marino")]
        [EnumMember(Value = "SM")]
        SM = 192,

        [Description("Sao Tome and Principe")]
        [EnumMember(Value = "ST")]
        ST = 193,

        [Description("Saudi Arabia")]
        [EnumMember(Value = "SA")]
        SA = 194,

        [Description("Scotland")]
        [EnumMember(Value = "SF")]
        SF = 195,

        [Description("Senegal")]
        [EnumMember(Value = "SN")]
        SN = 196,

        [Description("Seychelles")]
        [EnumMember(Value = "SC")]
        SC = 197,

        [Description("Sierra Leone")]
        [EnumMember(Value = "SL")]
        SL = 198,

        [Description("Singapore")]
        [EnumMember(Value = "SG")]
        SG = 199,

        [Description("Slovak Republic")]
        [EnumMember(Value = "SK")]
        SK = 200,

        [Description("Slovenia")]
        [EnumMember(Value = "SI")]
        SI = 201,

        [Description("Solomon Islands")]
        [EnumMember(Value = "SB")]
        SB = 202,

        [Description("Somalia")]
        [EnumMember(Value = "SO")]
        SO = 203,

        [Description("South Africa")]
        [EnumMember(Value = "ZA")]
        ZA = 204,

        [Description("South Georgia and South Sandwich Islands")]
        [EnumMember(Value = "GS")]
        GS = 205,

        [Description("Spain")]
        [EnumMember(Value = "ES")]
        ES = 206,

        [Description("Sri Lanka")]
        [EnumMember(Value = "LK")]
        LK = 207,

        [Description("Saint Barthelemy")]
        [EnumMember(Value = "BL")]
        BL = 208,

        [Description("Saint Christopher")]
        [EnumMember(Value = "SW")]
        SW = 209,

        [Description("Saint Eustatius")]
        [EnumMember(Value = "EU")]
        EU = 210,

        [Description("Saint John")]
        [EnumMember(Value = "UV")]
        UV = 211,

        [Description("Saint Martin")]
        [EnumMember(Value = "MF")]
        MF = 212,

        [Description("Sint Maarten")]
        [EnumMember(Value = "SX")]
        SX = 213,

        [Description("Saint Thomas")]
        [EnumMember(Value = "VL")]
        VL = 214,

        [Description("Sudan")]
        [EnumMember(Value = "SD")]
        SD = 215,

        [Description("Suriname")]
        [EnumMember(Value = "SR")]
        SR = 216,

        [Description("Svalbard and Jan Mayen Islands")]
        [EnumMember(Value = "SJ")]
        SJ = 217,

        [Description("Swaziland")]
        [EnumMember(Value = "SZ")]
        SZ = 218,

        [Description("Sweden")]
        [EnumMember(Value = "SE")]
        SE = 219,

        [Description("Switzerland")]
        [EnumMember(Value = "CH")]
        CH = 220,

        [Description("Syrian Arab Republic")]
        [EnumMember(Value = "SY")]
        SY = 221,

        [Description("Tahiti")]
        [EnumMember(Value = "TA")]
        TA = 222,

        [Description("Taiwan")]
        [EnumMember(Value = "TW")]
        TW = 223,

        [Description("Tajikistan")]
        [EnumMember(Value = "TJ")]
        TJ = 224,

        [Description("Tanzania")]
        [EnumMember(Value = "TZ")]
        TZ = 225,

        [Description("Thailand")]
        [EnumMember(Value = "TH")]
        TH = 226,

        [Description("Tinian")]
        [EnumMember(Value = "TI")]
        TI = 227,

        [Description("Togo")]
        [EnumMember(Value = "TG")]
        TG = 228,

        [Description("Tortola")]
        [EnumMember(Value = "TL")]
        TL = 229,

        [Description("Tokelau")]
        [EnumMember(Value = "TK")]
        TK = 230,

        [Description("Tonga")]
        [EnumMember(Value = "TO")]
        TO = 231,

        [Description("Trinidad and Tobago")]
        [EnumMember(Value = "TT")]
        TT = 232,

        [Description("Truk")]
        [EnumMember(Value = "TU")]
        TU = 233,

        [Description("Tunisia")]
        [EnumMember(Value = "TN")]
        TN = 234,

        [Description("Turkey")]
        [EnumMember(Value = "TR")]
        TR = 235,

        [Description("Turkmenistan")]
        [EnumMember(Value = "TM")]
        TM = 236,

        [Description("Turks Islands and Caicos Islands")]
        [EnumMember(Value = "TC")]
        TC = 237,

        [Description("Tuvalu")]
        [EnumMember(Value = "TV")]
        TV = 238,

        [Description("Uganda")]
        [EnumMember(Value = "UG")]
        UG = 239,

        [Description("Ukraine")]
        [EnumMember(Value = "UA")]
        UA = 240,

        [Description("Union Island")]
        [EnumMember(Value = "UI")]
        UI = 241,

        [Description("United Arab Emirates")]
        [EnumMember(Value = "AE")]
        AE = 242,

        [Description("United Kingdom")]
        [EnumMember(Value = "UK")]
        UK = 243,

        [Description("United States")]
        [EnumMember(Value = "US")]
        US = 244,

        [Description("US Minor Outlying Islands")]
        [EnumMember(Value = "UM")]
        UM = 245,

        [Description("Uruguay")]
        [EnumMember(Value = "UY")]
        UY = 246,

        [Description("Uzbekistan")]
        [EnumMember(Value = "UZ")]
        UZ = 247,

        [Description("Vanuatu")]
        [EnumMember(Value = "VU")]
        VU = 248,

        [Description("Vatican City State")]
        [EnumMember(Value = "VA")]
        VA = 249,

        [Description("Venezuela")]
        [EnumMember(Value = "VE")]
        VE = 250,

        [Description("Vietnam")]
        [EnumMember(Value = "VN")]
        VN = 251,

        [Description("Virgin Gorda")]
        [EnumMember(Value = "VR")]
        VR = 252,

        [Description("Virgin Islands (British)")]
        [EnumMember(Value = "VG")]
        VG = 253,

        [Description("Virgin Islands (US)")]
        [EnumMember(Value = "VI")]
        VI = 254,

        [Description("Wallis and Futuna Islands")]
        [EnumMember(Value = "WF")]
        WF = 255,

        [Description("Yemen")]
        [EnumMember(Value = "YE")]
        YE = 256,

        [Description("Serbia")]
        [EnumMember(Value = "RS")]
        RS = 257,

        [Description("Zaire")]
        [EnumMember(Value = "ZR")]
        ZR = 258,

        [Description("Zambia")]
        [EnumMember(Value = "ZM")]
        ZM = 259,

        [Description("Zimbabwe")]
        [EnumMember(Value = "ZW")]
        ZW = 260,
    }
}