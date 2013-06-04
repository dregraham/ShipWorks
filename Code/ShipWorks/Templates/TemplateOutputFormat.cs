using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Templates
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum TemplateOutputFormat
    {
        [Description("Html")]
        Html = 0,

        [Description("XML")]
        Xml = 1,

        [Description("Text")]
        Text = 2
    }
}
