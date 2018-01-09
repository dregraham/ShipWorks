using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Choxi.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Choxi Is Amazon Prime", "Choxi.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Choxi)]
    public class ChoxiIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
