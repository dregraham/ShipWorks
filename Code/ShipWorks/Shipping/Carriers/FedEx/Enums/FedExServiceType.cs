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
        [ApiValue("PRIORITY_OVERNIGHT")]
        PriorityOvernight = 0,

        [Description("FedEx Standard Overnight®")]
        [ApiValue("STANDARD_OVERNIGHT")]
        StandardOvernight = 1,

        [Description("FedEx First Overnight®")]
        [ApiValue("FIRST_OVERNIGHT")]
        FirstOvernight = 2,

        [Description("FedEx 2Day®")]
        [ApiValue("FEDEX_2_DAY")]
        FedEx2Day = 3,

        [Description("FedEx Express Saver®")]
        [ApiValue("FEDEX_EXPRESS_SAVER")]
        FedExExpressSaver = 4,

        [Description("FedEx International Priority®")]
        [ApiValue("INTERNATIONAL_PRIORITY")]
        InternationalPriority = 5,

        [Description("FedEx International Economy®")]
        [ApiValue("INTERNATIONAL_ECONOMY")]
        InternationalEconomy = 6,

        [Description("FedEx International First®")]
        [ApiValue("INTERNATIONAL_FIRST")]
        InternationalFirst = 7,

        [Description("FedEx 1Day® Freight")]
        [ApiValue("FEDEX_1_DAY_FREIGHT")]
        FedEx1DayFreight = 8,

        [Description("FedEx 2Day® Freight")]
        [ApiValue("FEDEX_2_DAY_FREIGHT")]
        FedEx2DayFreight = 9,

        [Description("FedEx 3Day® Freight")]
        [ApiValue("FEDEX_3_DAY_FREIGHT")]
        FedEx3DayFreight = 10,

        [Description("FedEx Ground®")]
        [ApiValue("FEDEX_GROUND")]
        FedExGround = 11,

        [Description("FedEx Home Delivery®")]
        [ApiValue("GROUND_HOME_DELIVERY")]
        GroundHomeDelivery = 12,

        [Description("FedEx International Priority® Freight")]
        [ApiValue("INTERNATIONAL_PRIORITY_FREIGHT")]
        InternationalPriorityFreight = 13,

        [Description("FedEx International Economy® Freight")]
        [ApiValue("INTERNATIONAL_ECONOMY_FREIGHT")]
        InternationalEconomyFreight = 14,

        [Description("FedEx SmartPost®")]
        [ApiValue("SMART_POST")]
        SmartPost = 15,

        [Description("FedEx Europe First International Priority®")]
        FedExEuropeFirstInternationalPriority = 17,

        [Description("FedEx 2Day® A.M.")]
        [ApiValue("FEDEX_2_DAY_AM")]
        FedEx2DayAM = 18,

        [Description("FedEx First Overnight® Freight")]
        [ApiValue("FEDEX_FIRST_FREIGHT")]
        FirstFreight = 19,

        [Description("FedEx One Rate® (First Overnight)")]
        [ApiValue("FIRST_OVERNIGHT")]
        OneRateFirstOvernight = 20,

        [Description("FedEx One Rate® (Priority Overnight)")]
        [ApiValue("PRIORITY_OVERNIGHT")]
        OneRatePriorityOvernight = 21,

        [Description("FedEx One Rate® (Standard Overnight)")]
        [ApiValue("STANDARD_OVERNIGHT")]
        OneRateStandardOvernight = 22,

        [Description("FedEx One Rate® (2Day)")]
        [ApiValue("FEDEX_2_DAY")]
        OneRate2Day = 23,

        [Description("FedEx One Rate® (2Day A.M.)")]
        [ApiValue("FEDEX_2_DAY_AM")]
        OneRate2DayAM = 24,

        [Description("FedEx One Rate® (Express Saver)")]
        [ApiValue("FEDEX_EXPRESS_SAVER")]
        OneRateExpressSaver = 25,

        [Description("FedEx Economy")]
        [ApiValue("FEDEX_EXPRESS_SAVER")]
        FedExEconomyCanada = 26,

        [Description("FedEx FIMS Mailview")]
        FedExFimsMailView = 27,

        [Description("FedEx International Ground®")]
        [ApiValue("FEDEX_GROUND")]
        FedExInternationalGround = 28,

        [Description("FedEx Next Day Afternoon")]
        [ApiValue("FEDEX_NEXT_DAY_AFTERNOON")]
        FedExNextDayAfternoon = 29,

        [Description("FedEx Next Day Early Morning")]
        [ApiValue("FEDEX_NEXT_DAY_EARLY_MORNING")]
        FedExNextDayEarlyMorning = 30,

        [Description("FedEx Next Day Mid Morning")]
        [ApiValue("FEDEX_NEXT_DAY_MID_MORNING")]
        FedExNextDayMidMorning = 31,

        [Description("FedEx Next Day End Of Day")]
        [ApiValue("FEDEX_NEXT_DAY_END_OF_DAY")]
        FedExNextDayEndOfDay = 32,

        [Description("FedEx Distance Deferred")]
        [ApiValue("FEDEX_DISTANCE_DEFERRED")]
        FedExDistanceDeferred = 33,

        [Description("FedEx Next Day Freight")]
        [ApiValue("FEDEX_NEXT_DAY_FREIGHT")]
        FedExNextDayFreight = 34,

        [Description("FedEx FIMS Mailview Lite")]
        FedExFimsMailViewLite = 35,

        [Description("FedEx FIMS Standard")]
        FedExFimsStandard = 36,

        [Description("FedEx FIMS Premium")]
        FedExFimsPremium = 37,

        [Description("FedEx International Priority® Express")]
        [ApiValue("INTERNATIONAL_PRIORITY_EXPRESS")]
        InternationalPriorityExpress = 38,

        [Description("FedEx Freight® Economy")]
        [ApiValue("FEDEX_FREIGHT_ECONOMY")]
        FedExFreightEconomy = 39,

        [Description("FedEx Freight® Priority")]
        [ApiValue("FEDEX_FREIGHT_PRIORITY")]
        FedExFreightPriority = 40,
    }
}
