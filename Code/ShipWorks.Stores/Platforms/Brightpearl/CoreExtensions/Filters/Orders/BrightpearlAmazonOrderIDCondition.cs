using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Brightpearl.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Brightpearl Amazon Order ID", "Brightpearl.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Brightpearl)]
    public class BrightpearlAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
