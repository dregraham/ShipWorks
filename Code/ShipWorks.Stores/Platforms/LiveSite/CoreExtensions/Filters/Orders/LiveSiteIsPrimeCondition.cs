using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LiveSite.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("LiveSite Is Amazon Prime", "LiveSite.IsPrime")]
    [ConditionStoreType(StoreTypeCode.LiveSite)]
    public class LiveSiteIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
