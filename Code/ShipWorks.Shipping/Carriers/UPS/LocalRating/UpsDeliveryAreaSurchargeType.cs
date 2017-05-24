using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsDeliveryAreaSurchargeType
    {
        [ApiValue("US 48 DAS")]
        Us48Das = 0,

        [ApiValue("US 48 DAS Extended")]
        Us48DasExtended = 1,

        [ApiValue("Remote HI")]
        UsRemoteHi = 2,

        [ApiValue("Remote AK")]
        UsRemoteAk = 3
    }
}
