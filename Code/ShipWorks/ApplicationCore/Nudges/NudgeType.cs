using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// An enumeration to indicate what the content of a nudge is for.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum NudgeType
    {
        /// <summary>
        /// Indicates the nudge is prompting the user to upgrade to the latest version of ShipWorks
        /// </summary>
        [Description("Upgrade ShipWorks")]
        [ApiValue("ShipWorksUpgrade")]
        ShipWorksUpgrade = 0,

        /// <summary>
        /// Prompt the user to create a Stamps.com account
        /// </summary>
        [Description("Create a Stamps.com account")]
        [ApiValue("RegisterStampsAccount")]
        RegisterStampsAccount = 1,

        /// <summary>
        /// Prompt the user to create a Stamps.com account
        /// </summary>
        [Description("Processing with Endicia maybe be restricted.")]
        [ApiValue("ProcessEndicia")]
        ProcessEndicia = 2,

        /// <summary>
        /// Prompt the user to create a Stamps.com account
        /// </summary>
        [Description("Processing with Express1 for Endicia maybe be restricted.")]
        [ApiValue("ProcessExpress1Endicia")]
        ProcessExpress1Endicia = 3
    }
}
