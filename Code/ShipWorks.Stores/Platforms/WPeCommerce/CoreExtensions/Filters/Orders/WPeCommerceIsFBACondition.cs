using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.WPeCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("WPeCommerce Is Fulfilled By Amazon", "WPeCommerce.IsFBA")]
    [ConditionStoreType(StoreTypeCode.WPeCommerce)]
    public class WPeCommerceIsFBACondition : GenericModuleIsFBACondition
    { }
}
