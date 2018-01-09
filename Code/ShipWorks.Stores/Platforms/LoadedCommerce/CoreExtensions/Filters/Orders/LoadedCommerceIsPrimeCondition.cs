using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LoadedCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("LoadedCommerce Is Amazon Prime", "LoadedCommerce.IsPrime")]
    [ConditionStoreType(StoreTypeCode.LoadedCommerce)]
    public class LoadedCommerceIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
