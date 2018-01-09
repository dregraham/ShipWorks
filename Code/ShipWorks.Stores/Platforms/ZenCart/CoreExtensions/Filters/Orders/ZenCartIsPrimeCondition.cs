using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ZenCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("ZenCart Is Amazon Prime", "ZenCart.IsPrime")]
    [ConditionStoreType(StoreTypeCode.ZenCart)]
    public class ZenCartIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
