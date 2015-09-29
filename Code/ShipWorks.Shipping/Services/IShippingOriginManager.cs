using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Interface for the shipping origin manager
    /// </summary>
    public interface IShippingOriginManager
    {
        /// <summary>
        /// Gets the origin address based on the given information
        /// </summary>
        PersonAdapter GetOriginAddress(long originId, long orderId, long accountId, ShipmentTypeCode shipmentType);
    }
}