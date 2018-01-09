using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OpenCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("OpenCart Is Amazon Prime", "OpenCart.IsPrime")]
    [ConditionStoreType(StoreTypeCode.OpenCart)]
    public class OpenCartIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
