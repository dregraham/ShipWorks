using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WooCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("WooCommerce Is Fulfilled By Amazon", "WooCommerce.IsFBA")]
    [ConditionStoreType(StoreTypeCode.WooCommerce)]
    public class WooCommerceIsFBACondition : GenericModuleIsFBACondition
    { }
}
