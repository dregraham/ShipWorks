using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WebShopManager.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("WebShopManager Amazon Order ID", "WebShopManager.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.WebShopManager)]
    public class WebShopManagerAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
