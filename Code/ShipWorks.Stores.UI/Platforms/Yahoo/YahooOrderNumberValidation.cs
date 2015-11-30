using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.UI.Platforms.Yahoo
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum YahooOrderNumberValidation
    {
        [Description("Valid")]
        [ImageResource("check16")]
        Valid,

        [Description("Invalid")]
        [ImageResource("error16")]
        Invalid,

        [Description("Validating")]
        [ImageResource("circle_ball_blue")]
        Validating
    }
}