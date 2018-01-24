using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.RevolutionParts.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("RevolutionParts Amazon Order ID", "RevolutionParts.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.RevolutionParts)]
    public class RevolutionPartsAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
