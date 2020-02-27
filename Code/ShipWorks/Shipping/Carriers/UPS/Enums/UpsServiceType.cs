using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.UPS.Enums
{
    /// <summary>
    /// UPS shipping services
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsServiceType
    {
        [Description("UPS Ground")]
        [ApiValue("ups_ground")]
        UpsGround = 0,

        [Description("UPS 3 Day Select®")]
        [ApiValue("ups_3_day_select")]
        Ups3DaySelect = 1,

        [Description("UPS 2nd Day Air®")]
        [ApiValue("ups_2nd_day_air")]
        Ups2DayAir = 2,

        [Description("UPS 2nd Day Air A.M.®")]
        [ApiValue("ups_2nd_day_air_am")]
        Ups2DayAirAM = 3,

        [Description("UPS Next Day Air®")]
        [ApiValue("ups_next_day_air")]
        UpsNextDayAir = 4,

        [Description("UPS Next Day Air Saver®")]
        [ApiValue("ups_next_day_air_saver")]
        UpsNextDayAirSaver = 5,

        [Description("UPS Next Day Air® Early")]
        [ApiValue("ups_next_day_air_early_am")]
        UpsNextDayAirAM = 6,

        [Description("UPS Worldwide Express®")]
        [ApiValue("ups_worldwide_express")]
        WorldwideExpress = 7,

        [Description("UPS Worldwide Express Plus®")]
        WorldwideExpressPlus = 8,

        [Description("UPS Worldwide Expedited®")]
        [ApiValue("ups_worldwide_expedited")]
        WorldwideExpedited = 9,

        [Description("UPS Worldwide Saver®")]
        [ApiValue("ups_worldwide_saver")]
        WorldwideSaver = 10,

        [Description("UPS Standard")]
        UpsStandard = 11,

        [Description("UPS First Class Innovations®")]
        UpsMailInnovationsFirstClass = 12,

        [Description("UPS Priority Mail Innovations®")]
        UpsMailInnovationsPriority = 13,

        [Description("UPS Expedited Mail Innovations®")]
        UpsMailInnovationsExpedited = 14,

        [Description("UPS Economy Mail Innovations®")]
        UpsMailInnovationsIntEconomy = 15,

        [Description("UPS Priority Mail Innovations®")]
        UpsMailInnovationsIntPriority = 16,

        [Description("UPS SurePost® Less than 1 LB")]
        UpsSurePostLessThan1Lb = 17,

        [Description("UPS SurePost® 1 LB or Greater")]
        UpsSurePost1LbOrGreater = 18,

        [Description("UPS SurePost® Bound Printed Matter")]
        UpsSurePostBoundPrintedMatter = 19,

        [Description("UPS SurePost® Media")]
        UpsSurePostMedia = 20,

        [Description("UPS Express®")]
        UpsExpress = 21,

        [Description("UPS Express Early A.M.®")]
        UpsExpressEarlyAm = 22,

        [Description("UPS Express Saver®")]
        UpsExpressSaver = 23,

        [Description("UPS Expedited®")]
        UpsExpedited = 24,

        [Description("UPS 3 Day Select® (Canada)")]
        Ups3DaySelectFromCanada = 25,

        [Description("UPS Worldwide Express Saver™ (Canada)")]
        UpsCaWorldWideExpressSaver = 26,

        [Description("UPS Worldwide Express Plus™ (Canada)")]
        UpsCaWorldWideExpressPlus = 27,

        [Description("UPS Worldwide Express™ (Canada)")]
        UpsCaWorldWideExpress = 28,

        [Description("UPS Second Day Air Intra")]
        Ups2ndDayAirIntra = 29

    }
}
