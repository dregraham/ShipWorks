using System.Reactive;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Manage which fields are available in the Order Lookup mode
    /// </summary>
    public interface IOrderLookupFieldManager
    {
        /// <summary>
        /// Show the manager
        /// </summary>
        Unit ShowManager();
    }
}
