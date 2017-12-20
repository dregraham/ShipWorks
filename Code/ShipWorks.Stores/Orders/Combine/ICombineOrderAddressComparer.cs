using System.Collections.Generic;
using Interapptive.Shared.Business;

namespace ShipWorks.Stores.Orders.Combine
{
    /// <summary>
    /// EqualityComparer for PersonAdapters used by the OrderCombine function
    /// </summary>
    public interface ICombineOrderAddressComparer : IEqualityComparer<PersonAdapter>
    {
    }
}