using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Fortune3.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Fortune3 Is Amazon Prime", "Fortune3.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Fortune3)]
    public class Fortune3IsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
