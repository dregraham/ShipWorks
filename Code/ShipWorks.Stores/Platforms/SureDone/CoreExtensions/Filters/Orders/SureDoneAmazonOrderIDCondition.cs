using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SureDone.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("SureDone Amazon Order ID", "SureDone.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.SureDone)]
    public class SureDoneAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
