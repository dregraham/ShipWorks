using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Shopp.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Shopp Is Amazon Prime", "Shopp.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Shopp)]
    public class ShoppIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
