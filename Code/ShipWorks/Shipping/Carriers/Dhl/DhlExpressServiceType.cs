using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Core.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Available DHL service types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DhlExpressServiceType
    {
        [Description("Express Worldwide")]
        [ApiValue("express_worldwide")]
        ExpressWorldWide = 1,

        [Description("Express Envelope")]
        [ApiValue("express_envelope")]
        ExpressEnvelope = 2
    }
}
