using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.InstanteStore.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("InstanteStore Is Amazon Prime", "InstanteStore.IsPrime")]
    [ConditionStoreType(StoreTypeCode.InstaStore)]
    public class InstanteStoreIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
