using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// Available Amazon SWA service types
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonSWAServiceType
    {
        [Description("Standard")]
        [ApiValue("standard")]
        Standard = 0,
    }
}
