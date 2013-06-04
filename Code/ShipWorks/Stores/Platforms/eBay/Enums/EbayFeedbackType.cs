using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores.Platforms.Ebay.Enums
{
    /// <summary>
    /// Represents type of ebay feedback
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EbayFeedbackType
    {
        [Description("")]
        None = -2,

        [Description("Unknown")]
        Unknown = -1,

        [Description("Positive")]
        Positive = 0,

        [Description("Neutral")]
        Neutral = 1,

        [Description("Negative")]
        Negative = 2,

        [Description("Withdrawn")]
        Withdrawn = 3,

        [Description("Withdrawn")]
        IndependentlyWithdrawn = 4,

        [Description("Custom")]
        CustomCode = 5,
    }
}
