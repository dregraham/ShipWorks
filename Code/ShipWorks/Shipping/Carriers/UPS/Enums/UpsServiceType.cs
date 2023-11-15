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
        [Description("UPS® Ground")]
        [SortOrder(1)]
        UpsGround = 0,

        [Description("UPS 3 Day Select®")]
        [SortOrder(3)]
        Ups3DaySelect = 1,

        [Description("UPS 2nd Day Air®")]
        [SortOrder(4)] 
        Ups2DayAir = 2,

        [Description("UPS 2nd Day Air A.M.®")]
        [SortOrder(5)] 
        Ups2DayAirAM = 3,

        [Description("UPS Next Day Air®")]
        [SortOrder(6)] 
        UpsNextDayAir = 4,

        [Description("UPS Next Day Air Saver®")]
        [SortOrder(7)] 
        UpsNextDayAirSaver = 5,

        [Description("UPS Next Day Air® Early")]
        [SortOrder(8)] 
        UpsNextDayAirAM = 6,

        [Description("UPS Worldwide Express®")]
        [SortOrder(9)] 
        WorldwideExpress = 7,

        [Description("UPS Worldwide Express Plus®")]
        [SortOrder(10)] WorldwideExpressPlus = 8,

        [Description("UPS Worldwide Expedited®")]
        [SortOrder(11)] WorldwideExpedited = 9,

        [Description("UPS Worldwide Saver®")]
        [SortOrder(12)]
        WorldwideSaver = 10,

        [Description("UPS Standard")]
        [SortOrder(13)] 
        UpsStandard = 11,

        [Description("UPS First Class Innovations®")]
        [SortOrder(14)] 
        UpsMailInnovationsFirstClass = 12,

        [Description("UPS Priority Mail Innovations®")]
        [SortOrder(15)] 
        UpsMailInnovationsPriority = 13,

        [Description("UPS Expedited Mail Innovations®")]
        [SortOrder(16)]
        UpsMailInnovationsExpedited = 14,

        [Description("UPS Economy Mail Innovations®")]
        [SortOrder(17)]
        UpsMailInnovationsIntEconomy = 15,

        [Description("UPS Priority Mail Innovations®")]
        [SortOrder(18)]
        UpsMailInnovationsIntPriority = 16,

        [Description("UPS SurePost® Less than 1 LB")]
        [SortOrder(19)]
        UpsSurePostLessThan1Lb = 17,

        [Description("UPS SurePost® 1 LB or Greater")]
        [SortOrder(20)] 
        UpsSurePost1LbOrGreater = 18,

        [Description("UPS SurePost® Bound Printed Matter")]
        [SortOrder(21)]
        UpsSurePostBoundPrintedMatter = 19,

        [Description("UPS SurePost® Media")]
        [SortOrder(22)]
        UpsSurePostMedia = 20,

        [Description("UPS Express®")]
        [SortOrder(23)]
        UpsExpress = 21,

        [Description("UPS Express Early A.M.®")]
        [SortOrder(24)]
        UpsExpressEarlyAm = 22,

        [Description("UPS Express Saver®")]
        [SortOrder(25)]
        UpsExpressSaver = 23,

        [Description("UPS Expedited®")]
        [SortOrder(26)]
        UpsExpedited = 24,

        [Description("UPS 3 Day Select® (Canada)")]
        [SortOrder(27)]
        Ups3DaySelectFromCanada = 25,

        [Description("UPS Worldwide Express Saver™ (Canada)")]
        [SortOrder(28)]
        UpsCaWorldWideExpressSaver = 26,

        [Description("UPS Worldwide Express Plus™ (Canada)")]
        [SortOrder(29)]
        UpsCaWorldWideExpressPlus = 27,

        [Description("UPS Worldwide Express™ (Canada)")]
        [SortOrder(30)]
        UpsCaWorldWideExpress = 28,

        [Description("UPS Second Day Air Intra")]
        [SortOrder(31)]
        Ups2ndDayAirIntra = 29,

        [Description("UPS® Ground Saver")]
        [SortOrder(2)]
        UpsGroundSaver = 30
    }
}
