using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LimeLightCRM.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("LimeLightCRM Amazon Order ID", "LimeLightCRM.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.LimeLightCRM)]
    public class LimeLightCRMAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
