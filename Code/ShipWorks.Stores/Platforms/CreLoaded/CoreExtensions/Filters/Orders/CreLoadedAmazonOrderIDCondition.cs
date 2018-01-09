using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CreLoaded.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("CreLoaded Amazon Order ID", "CreLoaded.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.CreLoaded)]
    public class CreLoadedAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
