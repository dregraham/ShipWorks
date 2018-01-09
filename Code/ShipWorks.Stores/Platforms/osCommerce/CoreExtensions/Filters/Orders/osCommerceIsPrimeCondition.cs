using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.osCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("osCommerce Is Amazon Prime", "osCommerce.IsPrime")]
    [ConditionStoreType(StoreTypeCode.osCommerce)]
    public class osCommerceIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
