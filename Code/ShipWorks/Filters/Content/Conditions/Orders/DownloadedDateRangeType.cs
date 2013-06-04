using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Filters.Content.Conditions.Orders
{
    /// <summary>
    /// Indicates if the "Downloaded" condition should specific a date range for when the download occurred
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DownloadedDateRangeType
    {
        [Description("On any date")]
        Anytime = 0,


        [Description("On the specifed date")]
        Specified = 1
    }
}
