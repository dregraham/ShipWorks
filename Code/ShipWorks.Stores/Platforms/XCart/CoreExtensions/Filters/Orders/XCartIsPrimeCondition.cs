using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.XCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("XCart Is Amazon Prime", "XCart.IsPrime")]
    [ConditionStoreType(StoreTypeCode.XCart)]
    public class XCartIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
