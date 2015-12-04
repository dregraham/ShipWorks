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
        [Description("Valid")]
        [ImageResource("check16")]
        Valid = 0,

        [Description("Invalid")]
        [ImageResource("error16")]
        Invalid = 1,

        [Description("Validating")]
        [ImageResource("circle_ball_blue")]
        Validating = 2
    }
}