using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Possible types of a ShipWorks template
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum TemplateType
    {
        [Description("Standard")]
        Standard = 0,

        [Description("Label")]
        Label = 1,

        [Description("Report")]
        Report = 2,

        [Description("Thermal")]
        Thermal = 3
    }
}
