using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Service for interacting with verified orders
    /// </summary>
    public interface IVerifiedOrderService
    {
        /// <summary>
        /// Save verified order data
        /// </summary>
        void Save(OrderEntity order);
    }
}
