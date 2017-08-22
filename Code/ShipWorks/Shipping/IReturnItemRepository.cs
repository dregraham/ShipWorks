using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Repository for return items
    /// </summary>
    public interface IReturnItemRepository
    {
        /// <summary>
        /// Loads the return data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="createIfNone">if set to <c>true</c> [create the return item if it does not exist].</param>
        void LoadReturnData(ShipmentEntity shipment, bool createIfNone);
    }
}