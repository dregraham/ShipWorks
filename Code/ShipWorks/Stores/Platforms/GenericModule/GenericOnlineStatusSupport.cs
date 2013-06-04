using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Options a generic store has for supporting online status
    /// </summary>
    public enum GenericOnlineStatusSupport
    {
        None = 0,

        StatusOnly = 1,

        StatusWithComment = 2,

        DownloadOnly = 3
    }
}
