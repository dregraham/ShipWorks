using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.UI.Platforms.Yahoo
{
    /// <summary>
    /// Validation status of a backup order number
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum YahooOrderNumberValidation
    {
        [Description("Not Validated")]
        NotValidated = 0,

        [Description("Validating")]
        Validating = 1,

        [Description("Valid")]
        Valid = 2,

        [Description("Invalid")]
        Invalid = 3
    }
}