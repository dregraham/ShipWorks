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
        [Description("Create a USPS account")]
        [ApiValue("RegisterStampsAccount")]
        RegisterUspsAccount = 1,

        /// <summary>
        /// Notify the user that processing with Endicia may be restricted
        /// </summary>
        [Description("Processing with Endicia may be restricted.")]
        [ApiValue("ProcessEndicia")]
        ProcessEndicia = 2,
        
        /// <summary>
        /// Notify the user that purchasing postage for Endicia may be restricted
        /// </summary>
        [Description("Purchasing postage for Endicia may be restricted.")]
        [ApiValue("PurchaseEndicia")]
        PurchaseEndicia = 4,        
    }
}
