using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderDynamics.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("OrderDynamics Amazon Order ID", "OrderDynamics.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.OrderDynamics)]
    public class OrderDynamicsAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
