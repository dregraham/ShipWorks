using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Controls the formatting of weight values in the weight text box
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum WeightDisplayFormat
    {
        [Description("Fractional Pounds (2.5 lbs)")]
        FractionalPounds,

        [Description("Pounds & Ounces (2 lbs 8 oz)")]
        PoundsOunces
    }
}
