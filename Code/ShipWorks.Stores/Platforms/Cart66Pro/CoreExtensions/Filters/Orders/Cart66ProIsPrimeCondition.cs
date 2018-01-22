using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Cart66Pro.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Cart66Pro Is Amazon Prime", "Cart66Pro.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Cart66Pro)]
    public class Cart66ProIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
