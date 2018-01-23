using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Cart66Lite.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Cart66Lite Is Amazon Prime", "Cart66Lite.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Cart66Lite)]
    public class Cart66LiteIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
