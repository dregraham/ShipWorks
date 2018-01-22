using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.PowersportsSupport.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("PowersportsSupport Amazon Order ID", "PowersportsSupport.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.PowersportsSupport)]
    public class PowersportsSupportAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
