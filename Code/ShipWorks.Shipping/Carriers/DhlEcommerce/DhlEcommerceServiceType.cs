using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce service types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DhlEcommerceServiceType
    {
        // TODO: DHLECommerce add MAX service types
        
        // International US
        [Description("DHL Globalmail Packet IPA")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlGlobalmailPacketIPA = 0,

        [Description("DHL Globalmail Packet ISAL")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlGlobalmailPacketISAL = 1,

        [Description("DHL Globalmail Packet Standard")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlGlobalmailPacketStandard = 2,

        [Description("DHL GlobalMail Business IPA")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlGlobalMailBusinessIPA = 3,

        [Description("DHL Parcel International Expedited - DDP")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlParcelInternationalExpeditedDDP = 4,

        [Description("DHL Parcel International Expedited - DDU")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlParcelInternationalExpeditedDDU = 5,

        [Description("DHL Parcel International Priority")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlParcelInternationalPriority = 6,

        [Description("DHL Parcel International Direct - DDP")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlParcelInternationalDirectDDP = 7,

        [Description("DHL Parcel International Direct - DDU")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlParcelInternationalDirectDDU = 8,

        [Description("DHL Packet International")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlPacketInternational = 9,

        [Description("DHL Packet Plus International")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlPacketPlusInternational = 10,

        [Description("DHL Parcel International Standard")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlParcelInternationalStandard = 11,

        // Domestic US

        [Description("DHL SmartMail Parcel Ground")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlSmartMailParcelGround = 12,

        [Description("DHL SmartMail Parcel Plus Ground")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlSmartMailParcelPlusGround = 13,

        [Description("DHL SmartMail Parcel Expedited")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlSmartMailParcelExpedited = 14,

        [Description("DHL SmartMail Parcel Plus Expedited")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlSmartMailParcelPlusExpedited = 15,

        [Description("DHL SmartMail BPM Expedited")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlSmartMailBPMExpedited = 16,

        [Description("DHL SmartMail BPM Ground")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlSmartMailBPMGround = 17,

        [Description("DHL SM Marketing Parcel Expedited")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlSMMarketingParcelExpedited = 18,

        [Description("DHL SM Marketing Parcel Ground")]
        [ApiValue("yyyyyyyyyyyyyy")]

        US_DhlSMMarketingParcelGround = 19,

        // CanadaServices(int’lonly)

        [Description("DHL Packet International")]
        [ApiValue("yyyyyyyyyyyyyy")]

        CA_DhlPacketInternational = 20,

        [Description("DHL Packet Plus International")]
        [ApiValue("yyyyyyyyyyyyyy")]

        CA_DhlPacketPlusInternational = 21,

        [Description("DHL Parcel International Standard")]
        [ApiValue("yyyyyyyyyyyyyy")]

        CA_DhlParcelInternationalStandard = 22,

        [Description("DHL Parcel International Direct Priority")]
        [ApiValue("yyyyyyyyyyyyyy")]

        CA_DhlParcelInternationalDirectPriority = 23,

        [Description("DHL Parcel International Direct Standard")]
        [ApiValue("yyyyyyyyyyyyyy")]
        CA_DhlParcelInternationalDirectStandard = 24,
    }
}
