using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ZenCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("ZenCart Amazon Order ID", "ZenCart.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.ZenCart)]
    public class ZenCartAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
