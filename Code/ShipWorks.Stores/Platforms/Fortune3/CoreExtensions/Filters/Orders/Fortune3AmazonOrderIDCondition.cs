using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Fortune3.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Fortune3 Amazon Order ID", "Fortune3.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Fortune3)]
    public class Fortune3AmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
