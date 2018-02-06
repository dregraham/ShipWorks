using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.nopCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("nopCommerce Is Amazon Prime", "nopCommerce.IsPrime")]
    [ConditionStoreType(StoreTypeCode.nopCommerce)]
    public class nopCommerceIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
