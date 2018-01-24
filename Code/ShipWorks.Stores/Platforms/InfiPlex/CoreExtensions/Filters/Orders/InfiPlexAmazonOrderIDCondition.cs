using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.InfiPlex.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("InfiPlex Amazon Order ID", "InfiPlex.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.InfiPlex)]
    public class InfiPlexAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
