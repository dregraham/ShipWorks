using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Choxi.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Choxi Amazon Order ID", "Choxi.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Choxi)]
    public class ChoxiAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
