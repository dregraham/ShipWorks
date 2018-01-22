using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderDesk.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("OrderDesk Amazon Order ID", "OrderDesk.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.OrderDesk)]
    public class OrderDeskAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
