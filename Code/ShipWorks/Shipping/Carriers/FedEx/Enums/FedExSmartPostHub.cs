using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExSmartPostHub
    {
        [Description("")]
        [ApiValue("none")]
        None = 0,

        [Description("Allentown, PA")]
        [ApiValue("allentown_pa")]
        AllentownPa = 5185, // ALPA

        [Description("Atlanta, GA")]
        [ApiValue("atlanta_ga")]
        AtlantaGa = 5303, // ATGA

        [Description("Baltimore, MD")]
        [ApiValue("baltimore_md")]
        BaltimoreMd = 5213, // BAMD

        [Description("Charlotte, NC")]
        [ApiValue("charlotte_nc")]
        CharlotteNc = 5281, // CHNC

        [Description("Chino, CA")]
        [ApiValue("chino_ca")]
        ChinoCa = 5929, // COCA

        [Description("Dallas, TX")]
        [ApiValue("dallas_tx")]
        DallasTx = 5751, // DLTX

        [Description("Denver, CO")]
        [ApiValue("denver_co")]
        DenverCo = 5802, // DNCO

        [Description("Detroit, MI")]
        [ApiValue("detroit_mi")]
        DetroitMi = 5481, // DTMI

        [Description("Edison, NJ")]
        [ApiValue("edison_nj")]
        EdisonNj = 5087, // EDNJ

        [Description("Grove City, OH")]
        [ApiValue("grove_city_oh")]
        GroveCityOh = 5431, // GCOH

        [Description("Groveport, OH")]
        [ApiValue("groveport_oh")]
        GroveportOh = 5436, // GPOH

        [Description("Houston, TX")]
        [ApiValue("houston_tx")]
        HoustonTx = 5771, // HOTX

        [Description("Indianapolis, IN")]
        [ApiValue("indianapolis_in")]
        IndianapolisIn = 5465, // ININ

        [Description("Kansas City, KS")]
        [ApiValue("kansas_city_ks")]
        KansasCityKs = 5648, // KCKS

        [Description("Los Angeles, CA")]
        [ApiValue("los_angeles_ca")]
        LosAngelesCa = 5902, // LACA

        [Description("Martinsburg, WV")]
        [ApiValue("martinsburg_wv")]
        MartinsburgWv = 5254, // MAWV

        [Description("Macungie, PA")]
        [ApiValue("macungie_pa")]
        MacungiePa = 5183, // MAPA

        [Description("Memphis, TN")]
        [ApiValue("memphis_tn")]
        MemphisTn = 5379, // METN

        [Description("Minneapolis, MN")]
        [ApiValue("minneapolis_mn")]
        MinneapolisMn = 5552, // MPMN

        [Description("New Berlin, WI")]
        [ApiValue("new_berlin_wi")]
        NewBerlinWi = 5531, // NBWI

        [Description("Newark, NJ")]
        [ApiValue("newark_ny")]
        NewarkNy = 5095, // NENJ

        [Description("Newburgh, NY")]
        [ApiValue("newburgh_ny")]
        NewburghNy = 5110, // NENY

        [Description("Northborough, MA")]
        [ApiValue("northborough_ma")]
        NorthboroughMa = 5015, // NOMA

        [Description("Orlando, FL")]
        [ApiValue("orlando_fl")]
        OrlandoFl = 5327, // ORFL

        [Description("Philadelphia, PA")]
        [ApiValue("philadelphia_pa")]
        PhiladelphiaPa = 5195, // PHPA

        [Description("Phoenix, AZ")]
        [ApiValue("phoneix_az")]
        PhoenixAz = 5854, // PHAZ

        [Description("Pittsburgh, PA")]
        [ApiValue("pittsburgh_pa")]
        PittsburghPa = 5150, // PTPA

        [Description("Reno, NV")]
        [ApiValue("reno_nv")]
        RenoNv = 5893, // RENV

        [Description("Sacramento, CA")]
        [ApiValue("sacramento_ca")]
        SacramentoCa = 5958, // SACA

        [Description("Salt Lake City, UT")]
        [ApiValue("salt_lake_city_ut")]
        SaltLakeCityUt = 5843, // SCUT

        [Description("Scranton, PA")]
        [ApiValue("scranton_pa")]
        ScrantonPa = 5186, // SCPA

        [Description("Seattle, WA")]
        [ApiValue("seattle_wa")]
        SeattleWa = 5983, // SEWA

        [Description("South Brunswick, NJ")]
        [ApiValue("south_brunswick_nj")]
        SouthBrunswickNj = 5097, // SBNJ

        [Description("St. Louis, MO")]
        [ApiValue("st_louis_mo")]
        StLouisMo = 5631, // STMO

        [Description("Tampa, FL")]
        [ApiValue("tampa_fl")]
        TampaFl = 5345, // TAFL

        [Description("Wheeling, IL")]
        [ApiValue("wheeling_il")]
        WheelingIl = 5602, // WHIL

        [Description("Windsor, CT")]
        [ApiValue("windsor_ct")]
        WindsorCt = 5061, // WICT
    }
}
