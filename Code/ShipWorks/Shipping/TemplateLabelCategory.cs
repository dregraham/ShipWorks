using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum TemplateLabelCategory
    {
        /// <summary>
        /// This is the primary label image that would be put on the package.
        /// </summary>
        [Description("Primary")]
        Primary,

        /// <summary>
        /// These are supplemental labels that are required in addition to the primary, such
        /// as customs documents or COD return labels.
        /// </summary>
        [Description("Supplemental")]
        Supplemental
    }
}
