using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Packaging types for the postal service
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PostalPackagingType
    {
        [Description("Package")]
        Package = 0,

        [Description("Envelope")]
        Envelope = 1,

        [Description("Large Envelope")]
        LargeEnvelope = 2,

        [Description("Flat Rate Small Box")]
        [SortOrder(3)]
        FlatRateSmallBox = 6,

        [Description("Flat Rate Medium Box")]
        FlatRateMediumBox = 4,

        [Description("Flat Rate Large Box")]
        FlatRateLargeBox = 5,

        [Description("Flat Rate Envelope")]
        [SortOrder(6)]
        FlatRateEnvelope = 3,

        [Description("Flat Rate Padded Envelope")]
        FlatRatePaddedEnvelope = 7,

        [Description("Flat Rate Legal Envelope")]
        [SortOrder(8)]
        FlatRateLegalEnvelope = 10,

        [Description("Regional Rate Box A")]
        [SortOrder(9)]
        RateRegionalBoxA = 8,

        [Description("Regional Rate Box B")]
        [SortOrder(10)]
        RateRegionalBoxB = 9,

        // Regional box c was discontinued in 2016
        [Description("Regional Rate Box C")]
        [Deprecated]
        RateRegionalBoxC = 11,

        [Description("Cubic")]
        Cubic = 12
    }
}
