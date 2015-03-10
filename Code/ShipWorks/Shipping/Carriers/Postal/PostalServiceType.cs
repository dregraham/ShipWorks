using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        DhlParcelStandard = 101,

        [Description("DHL SM Parcel Plus Expedited")]
        [ApiValue("DHLGMSMPARCELPLUSEXPEDITED")]
        DhlParcelPlusExpedited = 102,

        [Description("DHL SM Parcel Plus Ground")]
        [ApiValue("DHLGMSMPARCELPLUSGROUND")]
        DhlParcelPlusStandard = 103,

        [Description("DHL SM BPM Expedited")]
        [ApiValue("DHLGMSMBPMEXPEDITED")]
        DhlBpmExpedited = 104,

        [Description("DHL SM BPM Ground")]
        [ApiValue("DHLGMSMBPMGROUND")]
        DhlBpmStandard = 105,

        [Description("DHL SM Catalog Expedited")]
        [ApiValue("DHLGMSMCATALOGBPMEXPEDITED")]
        DhlCatalogExpedited = 106,

        [Description("DHL SM Catalog Ground")]
        [ApiValue("DHLGMSMCATALOGBPMGROUND")]
        DhlCatalogStandard = 107,

        [Description("DHL SM Media Mail Ground")]
        [ApiValue("DHLGMSMMEDIAMAILGROUND")]
        DhlMediaMailStandard = 108,

        [Description("DHL SM Marketing Ground")]
        [ApiValue("DHLGMSMMarketingParcelGround")]
        DhlMarketingStandard = 109,

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
    }
}
