using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.FedEx.Enums
{
    /// <summary>
    /// An enumeration to map to the FedEx NAFTA preference criteria values
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FedExNaftaPreferenceCriteria
    {
        [Description("A (wholly obtained or produced entirely in US/CA/MX)")]
        A = 0,

        [Description("B (non-NAFTA materials but satisfies the Rules of Origin)")]
        B = 1,

        [Description("C (produced in US/CA/MX exclusively from NAFTA materials)")]
        C = 2,

        [Description("D (qualifies for preferential NAFTA treatment)")]
        D = 3,

        [Description("E (qualifying automatic data processing goods/parts)")]
        E = 4,

        [Description("F (agricultural goods exported from U.S. to Mexico)")]
        F = 5

    }
}
