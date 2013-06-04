using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.EquaShip.Enums
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EquaShipServiceType
    {
        [Description("Ground")]
        Ground = 0,

        /*
        [Description("Express Mail")]
        ExpressMail = 1,

        [Description("Express Mail Flat Rate Envelope")]
        ExpressFlatRateEnvelope = 2,

        [Description("Priority Mail")]
        Priority = 3,

        [Description("Priority Mail Flate Rate Envelope")]
        ExpressPriorityFlatRateEnvelope = 4,

        [Description("Priority Mail Padded Flat Rate Envelope")]
        ExpressPriorityPaddedFlatRateEnvelope = 5,

        [Description("Priority Small Flat Rate Box")]
        ExpressPrioritySmallFlatRateBox = 6,

        [Description("Priority Medium Flat Rate Box")]
        ExpressPriorityMediumFlatRateBox = 7,

        [Description("Priority Large Flat Rate Box")]
        ExpressPriorityLargeFlatRateBox = 8, */

        [Description("(International Unavailable)")]
        International = 9
    }
}
