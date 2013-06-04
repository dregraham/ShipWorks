using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ShipWorks.Properties;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Used for logging the result of a download for a store
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DownloadResult
    {
        [Description("Success")]
        [ImageResource("check16")]
        Success = 0,

        [Description("Cancel")]
        [ImageResource("cancel16")]
        Cancel = 1,

        [Description("Error")]
        [ImageResource("error16")]
        Error = 2,

        [Description("Downloading")]
        [ImageResource("nav_down_green16")]
        Unfinished = 3
    }
}
