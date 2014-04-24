using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Defines states for whether the address is a military address
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum MilitaryAddressType
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("Military Address")]
        MilitaryAddress = 1,

        [Description("Not Military Address")]
        NotMilitaryAddress = 2
    }
}