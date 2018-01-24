using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SearchFit.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("SearchFit Amazon Order ID", "SearchFit.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.SearchFit)]
    public class SearchFitAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
