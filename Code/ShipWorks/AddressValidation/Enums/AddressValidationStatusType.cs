using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.AddressValidation.Enums
{
    /// <summary>
    /// Defines the various states of validation for addresses
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AddressValidationStatusType
    {
        [Description("Not Checked")]
        [ImageResource("help2_16")]
        NotChecked = 0,

        [Description("")]
        Pending = 1,

        [Description("Bad Address")]
        [ImageResource("error16")]
        BadAddress = 2,

        [Description("Good Address")]
        [ImageResource("check16")]
        Valid = 3,

        [Description("Fixed")]
        [ImageResource("check16")]
        Fixed = 4,

        [Description("Suggestion Ignored")]
        [ImageResource("check16")]
        SuggestionIgnored = 5,

        [Description("Has Suggestions")]
        [ImageResource("information16")]
        HasSuggestions = 6,

        [Description("Suggestion Selected")]
        [ImageResource("check16")]
        SuggestionSelected = 7,

        [Description("Error")]
        [ImageResource("error16")]
        Error = 8,

        [Description("International (Can't Validate)")]
        [ImageResource("earth16")]
        WillNotValidate = 9
    }
}
