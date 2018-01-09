using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WooCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("WooCommerce Is Amazon Prime", "WooCommerce.IsPrime")]
    [ConditionStoreType(StoreTypeCode.WooCommerce)]
    public class WooCommerceIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
