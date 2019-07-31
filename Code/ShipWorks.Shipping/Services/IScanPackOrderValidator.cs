using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Represents the ScanPackOrderValidator
    /// </summary>
    public interface IScanPackOrderValidator
    {
        /// <summary>
        /// Can the order be processed
        /// </summary>
        /// <remarks>
        /// validates if an order can be processed based on
        /// scanpack/warehouse settings and the orders state
        /// </remarks>
        Result CanProcessShipment(OrderEntity order);
    }
}
