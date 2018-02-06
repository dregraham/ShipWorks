using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Shopp.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Shopp Amazon Order ID", "Shopp.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Shopp)]
    public class ShoppAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
