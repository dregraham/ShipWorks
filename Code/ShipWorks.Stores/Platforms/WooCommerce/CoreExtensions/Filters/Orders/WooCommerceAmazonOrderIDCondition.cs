using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WooCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("WooCommerce Amazon Order ID", "WooCommerce.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.WooCommerce)]
    public class WooCommerceAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
