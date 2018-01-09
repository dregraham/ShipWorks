using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WebShopManager.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("WebShopManager Is Amazon Prime", "WebShopManager.IsPrime")]
    [ConditionStoreType(StoreTypeCode.WebShopManager)]
    public class WebShopManagerIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
