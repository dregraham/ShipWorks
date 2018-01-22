using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SearchFit.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("SearchFit Is Amazon Prime", "SearchFit.IsPrime")]
    [ConditionStoreType(StoreTypeCode.SearchFit)]
    public class SearchFitIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
