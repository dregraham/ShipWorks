using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WPeCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("WPeCommerce Is Amazon Prime", "WPeCommerce.IsPrime")]
    [ConditionStoreType(StoreTypeCode.WPeCommerce)]
    public class WPeCommerceIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
