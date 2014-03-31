using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Defines the various states of validation for addresses
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AddressValidationStatusType
    {
        [Description("Not Checked")]
        NotChecked = 0,

        [Description("Pending")]
        Pending = 1,

        [Description("Not Valid")]
        NotValid = 2,

        [Description("Valid")]
        Valid = 3,

        [Description("Adjusted")]
        Adjusted = 4,

        [Description("Overridden")]
        Overridden = 5,

        [Description("Needs Attentions")]
        NeedsAttention = 6,

        [Description("Suggestion Selected")]
        SuggestedSelected = 7
    }
}
