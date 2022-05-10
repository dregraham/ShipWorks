using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum CustomerLicenseKeyType
    {
        [Description("WebReg")]
        WebReg = 0,

        [Description("Legacy")]
        Legacy = 1
    }
}