using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CsCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("CsCart Is Amazon Prime", "CsCart.IsPrime")]
    [ConditionStoreType(StoreTypeCode.CsCart)]
    public class CsCartIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
