using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Defines states for whether the address is a PO Box
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum POBoxType
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("PO Box")]
        POBox = 1,

        [Description("Not PO Box")]
        NotPOBox = 2
    }
}