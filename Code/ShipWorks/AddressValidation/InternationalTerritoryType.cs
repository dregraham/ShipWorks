using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Defines states for whether the address is an international US territory
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum InternationalTerritoryType
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("International Territory")]
        InternationalTerritory = 1,

        [Description("Not International Territory")]
        NotInternationalTerritory = 2
    }
}