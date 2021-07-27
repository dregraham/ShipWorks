using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum CustomerLicenseKeyType
    {
        WebReg = 0,
        
        Legacy = 1
    }
}