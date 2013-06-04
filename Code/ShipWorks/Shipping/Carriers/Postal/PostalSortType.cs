using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// SortType codes for postal service
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PostalSortType
    {
        [Description("Single Piece")]
        SinglePiece = 0,

        [Description("Presorted")]
        Presorted = 1,

        [Description("Five Digit")]
        FiveDigit = 2,

        [Description("Three Digit")]
        ThreeDigit = 3,

        [Description("Bulk Mail Center (BMC)")]
        BMC = 4,

        [Description("Mixed BMC")]
        MixedBMC = 5,

        [Description("Non-Presorted")]
        Nonpresorted = 6,

        [Description("Sectional Center Facility (SCF)")]
        SCF = 7,
    }
}
