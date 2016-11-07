using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// GlobalPost services we support
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum GlobalPostServiceAvailability
    {
        None = 0,
        GlobalPost = 1,
        SmartSaver = 2
    }
}