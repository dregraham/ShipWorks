using System.Collections.Generic;
using Interapptive.Shared.Business;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// EqualityComparer for PersonAdapters used by the OrderCombine function
    /// </summary>
    public interface IOrderCombineAddressComparer : IEqualityComparer<PersonAdapter>
    {
    }
}