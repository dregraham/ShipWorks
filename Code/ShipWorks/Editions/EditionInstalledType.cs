using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Editions
{
    /// <summary>
    /// The type of edition that is intalled (as configured by app.config)
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EditionInstalledType
    {
        [Description("")]
        Standard,

        [Description("ups")]
        UpsDiscounted,

        [Description("endiciaEbay")]
        EndiciaEbay
    }
}
