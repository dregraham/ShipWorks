using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Shopperpress.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Shopperpress Amazon Order ID", "Shopperpress.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Shopperpress)]
    public class ShopperpressAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
