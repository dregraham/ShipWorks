using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CreLoaded.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("CreLoaded Is Amazon Prime", "CreLoaded.IsPrime")]
    [ConditionStoreType(StoreTypeCode.CreLoaded)]
    public class CreLoadedIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
