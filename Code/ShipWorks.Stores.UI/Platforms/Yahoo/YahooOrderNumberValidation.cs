using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.UI.Platforms.Yahoo
{
    /// <summary>
    /// Validation status of a backup order number
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public enum YahooOrderNumberValidation
    {
        NotValidated = 0,

        Validating = 1,

        Valid = 2,

        Invalid = 3
    }
}