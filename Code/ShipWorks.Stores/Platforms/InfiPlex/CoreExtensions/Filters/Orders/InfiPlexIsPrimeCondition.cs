using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.InfiPlex.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("InfiPlex Is Amazon Prime", "InfiPlex.IsPrime")]
    [ConditionStoreType(StoreTypeCode.InfiPlex)]
    public class InfiPlexIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
