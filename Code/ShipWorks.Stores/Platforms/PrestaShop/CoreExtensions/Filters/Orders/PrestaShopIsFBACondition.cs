using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.PrestaShop.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("PrestaShop Is Fulfilled By Amazon", "PrestaShop.IsFBA")]
    [ConditionStoreType(StoreTypeCode.PrestaShop)]
    public class PrestaShopIsFBACondition : GenericModuleIsFBACondition
    { }
}
