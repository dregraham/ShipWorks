using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AddressValidation.Enums
{
    /// <summary>
    /// Defines states for residential address status
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ValidationDetailStatusType
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("Yes")]
        Yes = 1,

        [Description("No")]
        No = 2
    }
}
