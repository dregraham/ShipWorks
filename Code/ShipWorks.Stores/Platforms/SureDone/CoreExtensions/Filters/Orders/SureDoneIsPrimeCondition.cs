using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SureDone.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("SureDone Is Amazon Prime", "SureDone.IsPrime")]
    [ConditionStoreType(StoreTypeCode.SureDone)]
    public class SureDoneIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
