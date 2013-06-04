using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Criteria options for which orders to download from CA
    /// </summary>
    public enum ChannelAdvisorDownloadCriteria
    {
        All = 0,
        Paid = 1,
        PaidNotShipped = 2,
        NotShipped = 3
    }
}
