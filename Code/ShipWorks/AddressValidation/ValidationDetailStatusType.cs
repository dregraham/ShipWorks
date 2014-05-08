using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Defines states for residential address status
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ValidationDetailStatusType
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("True")]
        True = 1,

        [Description("False")]
        False = 2
    }
}
