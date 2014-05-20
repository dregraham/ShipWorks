using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.ShipSense.Hashing
{
    public interface IShipSenseOrderItemKeyFactory
    {
        /// <summary>
        /// Creates a list of ShipSenseOrderItemKey instances for the given order items. The ShipSenseOrderItemKey instances
        /// will be configured with the names/values of the properties and attributes provided.
        /// </summary>
        IEnumerable<ShipSenseOrderItemKey> GetKeys(IEnumerable<OrderItemEntity> orderItems, List<string> propertyNames, List<string> attributeNames);
    }
}