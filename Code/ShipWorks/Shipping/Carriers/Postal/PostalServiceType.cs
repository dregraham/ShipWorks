using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PostalServiceType
    {
        [Description("Priority")]
        PriorityMail = 0,

        [Description("First Class")]
        FirstClass = 1,

        [Description("Priority Mail Express")]
        ExpressMail = 2,

        [Description("Media Mail")]
        MediaMail = 3,

        [Description("Library Mail")]
        LibraryMail = 4,

        // Renamed 1/18/2003 for the USPS changes
        [Description("Standard Post")]
        StandardPost = 5,

        [Deprecated]
        [Description("Bound Printed Matter")]
        BoundPrintedMatter = 6,

        [Deprecated]
        [Description("Global Express Guaranteed")]
        GlobalExpressGuaranteed = 7,

        [Deprecated]
        [Description("Global Express Guaranteed Non-Document")]
        GlobalExpressGuaranteedNonDocument = 8,

        [Description("International First")]
        InternationalFirst = 9,

        [Description("International Express")]
        InternationalExpress = 10,

        [Description("International Priority")]
        InternationalPriority = 11,

        [Deprecated]
        [Description("Express Mail (Premium)")]
        ExpressMailPremium = 12,

        [Description("Parcel Select")]
        ParcelSelect = 13,

        [Description("Critical Mail")]
        CriticalMail = 14,

        [Description("DHL SM Parcel Expedited")]
        [ApiValue("DHLGMSMPARCELSEXPEDITED")]
        DhlParcelExpedited = 100,

        [Description("DHL SM Parcel Ground")]
        [ApiValue("DHLGMSMPARCELSGROUND")]
        DhlParcelGround = 101,

        [Description("DHL SM Parcel Plus Expedited")]
        [ApiValue("DHLGMSMPARCELPLUSEXPEDITED")]
        DhlParcelPlusExpedited = 102,

        [Description("DHL SM Parcel Plus Ground")]
        [ApiValue("DHLGMSMPARCELPLUSGROUND")]
        DhlParcelPlusGround = 103,

        [Description("DHL SM BPM Expedited")]
        [ApiValue("DHLGMSMBPMEXPEDITED")]
        DhlBpmExpedited = 104,

        [Description("DHL SM BPM Ground")]
        [ApiValue("DHLGMSMBPMGROUND")]
        DhlBpmGround = 105,

        [Description("DHL SM Catalog Expedited")]
        [ApiValue("DHLGMSMCATALOGBPMEXPEDITED")]
        DhlCatalogExpedited = 106,

        [Description("DHL SM Catalog Ground")]
        [ApiValue("DHLGMSMCATALOGBPMGROUND")]
        DhlCatalogGround = 107,

        [Description("DHL SM Media Mail Ground")]
        [ApiValue("DHLGMSMMEDIAMAILGROUND")]
        DhlMediaMailGround = 108,

        [Description("DHL SM Marketing Ground")]
        [ApiValue("DHLGMSMMarketingParcelGround")]
        DhlMarketingGround = 109,

        [Description("DHL SM Marketing Expedited")]
        [ApiValue("DHLGMSMMarketingParcelExpedited")]
        DhlMarketingExpedited = 110,

        [Description("Consolidator Label")]
        [ApiValue("ParcelSelect")]
        ConsolidatorDomestic = 200,

        [Description("Consolidator (International)")]
        [ApiValue("CONSINTL")]
        ConsolidatorInternational = 201,

        [Description("Consolidator (IPA)")]
        [ApiValue("IPA")]
        ConsolidatorIpa = 202,

        [Description("Consolidator (ISAL)")]
        [ApiValue("ISAL")]
        ConsolidatorIsal = 203,

        [Description("Commercial ePacket")]
        [ApiValue("CommercialePacket")]
        CommercialePacket = 204,

        [Description("Asendia IPA")]
        AsendiaIpa = 210,

        [Description("Asendia ISAL")]
        AsendiaIsal = 211,

        [Description("Asendia ePacket")]
        AsendiaePacket = 212,

        [Description("Asendia Generic")]
        AsendiaGeneric = 213,

        [Description("DHL Packet IPA")]
        DhlPacketIpa = 214,

        [Description("DHL Packet ISAL")]
        DhlPacketIsal = 215,

        [Description("Globegistics IPA")]
        GlobegisticsIpa = 216,

        [Description("Globegistics ISAL")]
        GlobegisticsIsal = 217,

        [Description("Globegistics ePacket")]
        GlobegisticsePacket = 218,

        [Description("Globegistics Generic")]
        GlobegisticsGeneric = 219,

        [Description("International Bonded Couriers IPA")]
        InternationalBondedCouriersIpa = 220,

        [Description("International Bonded Couriers ISAL")]
        InternationalBondedCouriersIsal = 221,

        [Description("International Bonded Couriers ePacket")]
        InternationalBondedCouriersePacket = 222,

        [Description("RRD IPA")]
        RrdIpa = 223,

        [Description("RRD ISAL")]
        RrdIsal = 224,

        [Description("RRD EPS (ePacket Service)")]
        RrdEpsePacketService = 225,
    }
}
