using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WPeCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("WPeCommerce Amazon Order ID", "WPeCommerce.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.WPeCommerce)]
    public class WPeCommerceAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
