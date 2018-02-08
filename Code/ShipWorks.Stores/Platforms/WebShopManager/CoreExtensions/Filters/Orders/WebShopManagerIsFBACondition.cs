using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WebShopManager.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("WebShopManager Is Fulfilled By Amazon", "WebShopManager.IsFBA")]
    [ConditionStoreType(StoreTypeCode.WebShopManager)]
    public class WebShopManagerIsFBACondition : GenericModuleIsFBACondition
    { }
}
