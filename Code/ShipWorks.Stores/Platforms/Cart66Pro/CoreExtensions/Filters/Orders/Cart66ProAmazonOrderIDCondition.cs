using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Cart66Pro.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Cart66Pro Amazon Order ID", "Cart66Pro.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Cart66Pro)]
    public class Cart66ProAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
