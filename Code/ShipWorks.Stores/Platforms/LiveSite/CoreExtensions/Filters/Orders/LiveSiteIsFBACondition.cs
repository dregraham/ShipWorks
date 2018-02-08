using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LiveSite.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("LiveSite Is Fulfilled By Amazon", "LiveSite.IsFBA")]
    [ConditionStoreType(StoreTypeCode.LiveSite)]
    public class LiveSiteIsFBACondition : GenericModuleIsFBACondition
    { }
}
