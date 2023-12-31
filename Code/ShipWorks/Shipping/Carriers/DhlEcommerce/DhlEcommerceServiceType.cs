﻿using System.ComponentModel;
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
        // International US
        [Description("DHL Globalmail Packet IPA")]
        [ApiValue("globalmail_packet_ipa")]
        [InternationalService]
        US_DhlGlobalmailPacketIPA = 0,

        [Description("DHL Globalmail Packet ISAL")]
        [ApiValue("globalmail_packet_isal")]
        [InternationalService]
        US_DhlGlobalmailPacketISAL = 1,

        [Description("DHL Globalmail Packet Standard")]
        [ApiValue("globalmail_packet_standard")]
        [InternationalService]
        US_DhlGlobalmailPacketStandard = 2,

        [Description("DHL GlobalMail Business IPA")]
        [ApiValue("dhl_globalmail_business_ipa")]
        [InternationalService]
        US_DhlGlobalMailBusinessIPA = 3,

        [Description("DHL Parcel International Expedited - DDP")]
        [ApiValue("globalmail_parcel_direct_express_ddp")]
        [InternationalService]
        US_DhlParcelInternationalExpeditedDDP = 4,

        [Description("DHL Parcel International Expedited - DDU")]
        [ApiValue("globalmail_parcel_direct_express_ddu")]
        [InternationalService]
        US_DhlParcelInternationalExpeditedDDU = 5,

        [Description("DHL Parcel International Priority")]
        [ApiValue("globalmail_parcel_priority")]
        [InternationalService]
        US_DhlParcelInternationalPriority = 6,

        [Description("DHL Parcel International Direct - DDP")]
        [ApiValue("globalmail_parcel_direct_ddp")]
        [InternationalService]
        US_DhlParcelInternationalDirectDDP = 7,

        [Description("DHL Parcel International Direct - DDU")]
        [ApiValue("globalmail_parcel_direct_ddu")]
        [InternationalService]
        US_DhlParcelInternationalDirectDDU = 8,

        [Description("DHL Packet International")]
        [ApiValue("globalmail_packet_priority")]
        [InternationalService]
        US_DhlPacketInternational = 9,

        [Description("DHL Packet Plus International")]
        [ApiValue("globalmail_packet_plus")]
        [InternationalService]
        US_DhlPacketPlusInternational = 10,

        [Description("DHL Parcel International Standard")]
        [ApiValue("globalmail_parcel_standard")]
        [InternationalService]
        US_DhlParcelInternationalStandard = 11,

        // Domestic US

        [Description("DHL SmartMail Parcel Ground")]
        [ApiValue("smartmail_parcels_ground")]
        US_DhlSmartMailParcelGround = 12,

        [Description("DHL SmartMail Parcel Plus Ground")]
        [ApiValue("smartmail_parcel_plus_ground")]
        US_DhlSmartMailParcelPlusGround = 13,

        [Description("DHL SmartMail Parcel Expedited")]
        [ApiValue("smartmail_parcels_expedited")]
        US_DhlSmartMailParcelExpedited = 14,

        [Description("DHL SmartMail Parcel Plus Expedited")]
        [ApiValue("smartmail_parcel_plus_expedited")]
        US_DhlSmartMailParcelPlusExpedited = 15,

        [Description("DHL SmartMail BPM Expedited")]
        [ApiValue("dhl_smartmail_bpm_expedited")]
        US_DhlSmartMailBPMExpedited = 16,

        [Description("DHL SmartMail BPM Ground")]
        [ApiValue("dhl_smartmail_bpm_ground")]
        US_DhlSmartMailBPMGround = 17,

        [Description("DHL SM Marketing Parcel Expedited")]
        [ApiValue("dhl_sm_marketing_parcel_expedited")]
        US_DhlSMMarketingParcelExpedited = 18,

        [Description("DHL SM Marketing Parcel Ground")]
        [ApiValue("dhl_sm_marketing_parcel_ground")]
        US_DhlSMMarketingParcelGround = 19,

        [Description("DHL SM Parcel Expedited Max")]
        [ApiValue("dhl_sm_parcel_expedited_max")]
        US_DhlSMParcelExpeditedMax = 23,

        // CanadaServices(int’lonly)

        [Description("DHL Parcel International Direct Priority")]
        [ApiValue("dhl_parcel_international_direct_priority")]
        [InternationalService("CA")]
        CA_DhlParcelInternationalDirectPriority = 24,

        [Description("DHL Parcel International Direct Standard")]
        [ApiValue("dhl_parcel_international_direct_standard")]
        [InternationalService("CA")]
        CA_DhlParcelInternationalDirectStandard = 25,
    }
}
