using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// What initiated a download
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum DownloadInitiatedBy
    {
        /// <summary>
        /// Automatic download issued by ShipWorks
        /// </summary>
        [Description("ShipWorks")]
        ShipWorks = 0,

        /// <summary>
        /// User manually clicked the download button
        /// </summary>
        [Description("User")]
        User = 1
    }
}
