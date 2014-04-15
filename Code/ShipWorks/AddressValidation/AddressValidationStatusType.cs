using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Defines the various states of validation for addresses
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AddressValidationStatusType
    {
        [Description("Not Checked")]
        [ImageResource("cancel16")]
        NotChecked = 0,

        [Description("Pending")]
        [ImageResource("nav_redo_yellow")]
        Pending = 1,

        [Description("Not Valid")]
        [ImageResource("error16")]
        NotValid = 2,

        [Description("Valid")]
        [ImageResource("check16")]
        Valid = 3,

        [Description("Adjusted")]
        [ImageResource("check16")]
        Adjusted = 4,

        [Description("Overridden")]
        [ImageResource("check16")]
        Overridden = 5,

        [Description("Needs Attention")]
        [ImageResource("error16")]
        NeedsAttention = 6,

        [Description("Suggestion Selected")]
        [ImageResource("check16")]
        SuggestedSelected = 7,

        [Description("Error")]
        [ImageResource("error16")]
        Error = 8
    }
}
