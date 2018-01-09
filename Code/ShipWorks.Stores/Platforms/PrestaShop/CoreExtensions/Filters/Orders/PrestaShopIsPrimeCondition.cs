using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.PrestaShop.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("PrestaShop Is Amazon Prime", "PrestaShop.IsPrime")]
    [ConditionStoreType(StoreTypeCode.PrestaShop)]
    public class PrestaShopIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
