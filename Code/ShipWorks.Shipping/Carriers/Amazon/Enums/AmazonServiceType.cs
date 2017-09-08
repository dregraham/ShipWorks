using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Amazon.Enums
{
    /// <summary>
    /// Hardcoded list of AmazonServiceTypes
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonServiceType
    {
        [ApiValue("FEDEX_PTP_SECOND_DAY")]
        [Description("FedEx 2Day®")]
        FedEx2Day = 0,

        [ApiValue("FEDEX_PTP_SECOND_DAY_AM")]
        [Description("FedEx 2Day®A.M.")]
        FedEx2DayAM = 1,

        [ApiValue("FEDEX_PTP_EXPRESS_SAVER")]
        [Description("FedEx Express Saver®")]
        FedExExpressSaver = 2,

        [ApiValue("FEDEX_PTP_GROUND")]
        [Description("FedEx Ground®")]
        FedExGround = 3,

        [ApiValue("FEDEX_PTP_PRIORITY_OVERNIGHT")]
        [Description("FedEx Priority Overnight®")]
        FedExPriorityOvernight = 4,

        [ApiValue("FEDEX_PTP_STANDARD_OVERNIGHT")]
        [Description("FedEx Standard Overnight®")]
        FedExStandardOvernight = 5,

        [ApiValue("UPS_PTP_2ND_DAY_AIR")]
        [Description("UPS 2nd Day Air")]
        UPS2ndDayAir = 6,

        [ApiValue("UPS_PTP_3DAY_SELECT")]
        [Description("UPS 3 Day Select")]
        UPS3DaySelect = 7,

        [ApiValue("UPS_PTP_GND")]
        [Description("UPS Ground")]
        UPSGround = 8,

        [ApiValue("UPS_PTP_NEXT_DAY_AIR")]
        [Description("UPS Next Day Air")]
        UPSNextDayAir = 9,

        [ApiValue("UPS_PTP_NEXT_DAY_AIR_SAVER")]
        [Description("UPS Next Day Air Saver")]
        UPSNextDayAirSaver = 10,

        [ApiValue("USPS_PTP_PSBN")]
        [Description("USPS Parcel Select")]
        USPSParcelSelect = 11,

        [ApiValue("USPS_PTP_EXP_LFRE")]
        [Description("USPS Priority Mail Express Legal Flat Rate Envelope")]
        USPSPriorityMailExpressLegalFlatRateEnvelope = 12,

        [ApiValue("USPS_PTP_EXP")]
        [Description("USPS Priority Mail Express®")]
        USPSPriorityMailExpress = 13,

        [ApiValue("USPS_PTP_EXP_FRE")]
        [Description("USPS Priority Mail Express® Flat Rate Envelope")]
        USPSPriorityMailExpressFlatRateEnvelope = 14,

        [ApiValue("USPS_PTP_PRI_LFRE")]
        [Description("USPS Priority Mail Legal Flat Rate Envelope")]
        USPSPriorityMailLegalFlatRateEnvelope = 15,

        [ApiValue("USPS_PTP_PRI_PFRE")]
        [Description("USPS Priority Mail Padded Flat Rate Envelope")]
        USPSPriorityMailPaddedFlatRateEnvelope = 16,

        [ApiValue("USPS_PTP_PRI_RA")]
        [Description("USPS Priority Mail Regional Rate Box A")]
        USPSPriorityMailRegionalRateBoxA = 17,

        [ApiValue("USPS_PTP_PRI_RB")]
        [Description("USPS Priority Mail Regional Rate Box B")]
        USPSPriorityMailRegionalRateBoxB = 18,

        [ApiValue("USPS_PTP_PRI")]
        [Description("USPS Priority Mail®")]
        USPSPriorityMail = 19,

        [ApiValue("USPS_PTP_PRI_MFRB")]
        [Description("USPS Priority Mail® Flat Rate Box")]
        USPSPriorityMailFlatRateBox = 20,

        [ApiValue("USPS_PTP_PRI_FRE")]
        [Description("USPS Priority Mail® Flat Rate Envelope")]
        USPSPriorityMailFlatRateEnvelope = 21,

        [ApiValue("USPS_PTP_PRI_LFRB")]
        [Description("USPS Priority Mail® Large Flat Rate Box")]
        USPSPriorityMailLargeFlatRateBox = 22,

        [ApiValue("USPS_PTP_PRI_SFRB")]
        [Description("USPS Priority Mail® Small Flat Rate Box")]
        USPSPriorityMailSmallFlatRateBox = 23,
    }
}