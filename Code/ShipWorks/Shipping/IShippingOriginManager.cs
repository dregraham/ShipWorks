using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
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

        /// <summary>
        /// Gets the origin address based on the origin id and shipment
        /// </summary>
        PersonAdapter GetOriginAddress(long originId, ShipmentEntity shipment);
    }
}