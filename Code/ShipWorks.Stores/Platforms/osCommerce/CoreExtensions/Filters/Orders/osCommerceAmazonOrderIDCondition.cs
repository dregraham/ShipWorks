using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.osCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("osCommerce Amazon Order ID", "osCommerce.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.osCommerce)]
    public class osCommerceAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
