using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

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
        [ImageResource("circle_ball_blue")]
        Validating = 1,

        [Description("Valid")]
        [ImageResource("check16")]
        Valid = 2,

        [Description("Invalid")]
        [ImageResource("error16")]
        Invalid = 3
    }
}