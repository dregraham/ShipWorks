using System;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Flags for downloading OMS orders
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [Flags]
    public enum MarketplaceAdvisorOmsFlagTypes
    {
        None = 0x0,
        Custom1 = 0x00000001,
        Custom2 = 0x00000002,
        Custom3 = 0x00000004,
        Custom4 = 0x00000008,
        Custom5 = 0x00000010,
        Custom6 = 0x00000020,
        Custom7 = 0x00000040,
        Custom8 = 0x00000080,
        Custom9 = 0x00000100,
        Custom10 = 0x00000200,
        PaymentCleared = 0x01000000,
        PayMethodChanged = 0x02000000,
        PartiallyShipped = 0x04000000
    }
}
