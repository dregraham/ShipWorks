using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderBot.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("OrderBot Amazon Order ID", "OrderBot.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.OrderBot)]
    public class OrderBotAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
