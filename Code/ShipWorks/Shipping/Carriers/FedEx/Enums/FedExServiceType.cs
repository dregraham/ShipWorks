using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// Valid FedEx service type values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExServiceType
    {
        [Description("FedEx Priority Overnight®")]
        [ApiValue("fedex_priority_overnight")]
        PriorityOvernight = 0,

        [Description("FedEx Standard Overnight®")]
        [ApiValue("fedex_standard_overnight")]
        StandardOvernight = 1,

        [Description("FedEx First Overnight®")]
        [ApiValue("fedex_first_overnight")]
        FirstOvernight = 2,

        [Description("FedEx 2Day®")]
        [ApiValue("fedex_2day")]
        FedEx2Day = 3,

        [Description("FedEx Express Saver®")]
        [ApiValue("fedex_express_saver")]
        FedExExpressSaver = 4,

        [Description("FedEx International Priority®")]
        [ApiValue("fedex_international_priority")]
        InternationalPriority = 5,

        [Description("FedEx International Economy®")]
        [ApiValue("fedex_international_economy")]
        InternationalEconomy = 6,

        [Description("FedEx International First®")]
        [ApiValue("fedex_international_first")]
        InternationalFirst = 7,

        [Description("FedEx 1Day® Freight")]
        [ApiValue("fedex_1_day_freight")]
        FedEx1DayFreight = 8,

        [Description("FedEx 2Day® Freight")]
        [ApiValue("fedex_2_day_freight")]
        FedEx2DayFreight = 9,

        [Description("FedEx 3Day® Freight")]
        [ApiValue("fedex_3_day_freight")]
        FedEx3DayFreight = 10,

        [Description("FedEx Ground®")]
        [ApiValue("fedex_ground")]
        FedExGround = 11,

        [Description("FedEx Home Delivery®")]
        [ApiValue("fedex_home_delivery")]
        GroundHomeDelivery = 12,

        [Description("FedEx International Priority® Freight")]
        [ApiValue("fedex_international_priority_freight")]
        InternationalPriorityFreight = 13,

        [Description("FedEx International Economy® Freight")]
        [ApiValue("fedex_international_economy_freight")]
        InternationalEconomyFreight = 14,

        [Description("FedEx SmartPost®")]
        SmartPost = 15,

        [Description("FedEx Europe First International Priority®")]
        [ApiValue("fedex_europe_first")]
        FedExEuropeFirstInternationalPriority = 17,

        [Description("FedEx 2Day® A.M.")]
        [ApiValue("fedex_2day_am")]
        FedEx2DayAM = 18,

        [Description("FedEx First Overnight® Freight")]
        [ApiValue("fedex_first_overnight_freight")]
        FirstFreight = 19,

        [Description("FedEx One Rate® (First Overnight)")]
        [ApiValue("fedex_first_overnight")]
        OneRateFirstOvernight = 20,

        [Description("FedEx One Rate® (Priority Overnight)")]
        [ApiValue("fedex_priority_overnight")]
        OneRatePriorityOvernight = 21,

        [Description("FedEx One Rate® (Standard Overnight)")]
        [ApiValue("fedex_standard_overnight")]
        OneRateStandardOvernight = 22,

        [Description("FedEx One Rate® (2Day)")]
        [ApiValue("fedex_2day")]
        OneRate2Day = 23,

        [Description("FedEx One Rate® (2Day A.M.)")]
        [ApiValue("fedex_2day_am")]
        OneRate2DayAM = 24,

        [Description("FedEx One Rate® (Express Saver)")]
        [ApiValue("fedex_express_saver")]
        OneRateExpressSaver = 25,

        [Description("FedEx Economy")]
        [ApiValue("fedex_express_saver")]
        FedExEconomyCanada = 26,

        [Description("FedEx FIMS Mailview")]
        FedExFimsMailView = 27,

        [Description("FedEx International Ground®")]
        [ApiValue("fedex_ground_international")]
        FedExInternationalGround = 28,

        [Description("FedEx Next Day Afternoon")]
        FedExNextDayAfternoon = 29,

        [Description("FedEx Next Day Early Morning")]
        FedExNextDayEarlyMorning = 30,

        [Description("FedEx Next Day Mid Morning")]
        FedExNextDayMidMorning = 31,

        [Description("FedEx Next Day End Of Day")]
        FedExNextDayEndOfDay = 32,

        [Description("FedEx Distance Deferred")]
        FedExDistanceDeferred = 33,

        [Description("FedEx Next Day Freight")]
        FedExNextDayFreight = 34,

        [Description("FedEx FIMS Mailview Lite")]
        FedExFimsMailViewLite = 35,

        [Description("FedEx FIMS Standard")]
        FedExFimsStandard = 36,

        [Description("FedEx FIMS Premium")]
        FedExFimsPremium = 37,

        [Description("FedEx International Priority® Express")]
        [ApiValue("fedex_international_priority_express")]
        InternationalPriorityExpress = 38,

        [Description("FedEx Freight® Economy")]
        [ApiValue("fedex_freight_economy")]
        FedExFreightEconomy = 39,

        [Description("FedEx Freight® Priority")]
        [ApiValue("fedex_freight_priority")]
        FedExFreightPriority = 40,

        [Description("FedEx SmartPost® Returns")]
        [ApiValue("fedex_smartpost_returns")]
        FedExSmartPostReturns = 41,
    }
}
