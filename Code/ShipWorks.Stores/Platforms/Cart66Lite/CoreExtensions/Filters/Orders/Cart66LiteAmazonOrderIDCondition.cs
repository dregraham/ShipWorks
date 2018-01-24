using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Cart66Lite.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Cart66Lite Amazon Order ID", "Cart66Lite.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Cart66Lite)]
    public class Cart66LiteAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
