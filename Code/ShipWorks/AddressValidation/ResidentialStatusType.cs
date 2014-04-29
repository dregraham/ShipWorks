using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Defines states for residential address status
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ResidentialStatusType
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("Residential")]
        Residential = 1,

        [Description("Commercial")]
        Commercial = 2
    }
}
