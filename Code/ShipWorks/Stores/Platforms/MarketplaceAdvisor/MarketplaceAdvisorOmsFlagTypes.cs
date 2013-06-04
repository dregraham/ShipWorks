using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Flags for downloading OMS orders
    /// </summary>
    [Flags]
    public enum MarketplaceAdvisorOmsFlagTypes
    {
        None             = 0x0,
        Custom1          = 0x00000001,
        Custom2          = 0x00000002,
        Custom3          = 0x00000004,
        Custom4          = 0x00000008,
        Custom5          = 0x00000010,
        Custom6          = 0x00000020,
        Custom7          = 0x00000040,
        Custom8          = 0x00000080,
        Custom9          = 0x00000100,
        Custom10         = 0x00000200,
        PaymentCleared   = 0x01000000,
        PayMethodChanged = 0x02000000,
        PartiallyShipped = 0x04000000
 }
}
