using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LiveSite.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("LiveSite Amazon Order ID", "LiveSite.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.LiveSite)]
    public class LiveSiteAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
