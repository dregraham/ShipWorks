using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SearchFit.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("SearchFit Is Fulfilled By Amazon", "SearchFit.IsFBA")]
    [ConditionStoreType(StoreTypeCode.SearchFit)]
    public class SearchFitIsFBACondition : GenericModuleIsFBACondition
    { }
}
