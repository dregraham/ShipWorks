using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Miva.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Miva Is Amazon Prime", "Miva.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Miva)]
    public class MivaIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
