using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Zenventory.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Zenventory Amazon Order ID", "Zenventory.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Zenventory)]
    public class ZenventoryAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
