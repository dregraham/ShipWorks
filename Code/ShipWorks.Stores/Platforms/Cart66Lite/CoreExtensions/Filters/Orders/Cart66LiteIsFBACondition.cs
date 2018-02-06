using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Cart66Lite.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Cart66Lite Is Fulfilled By Amazon", "Cart66Lite.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Cart66Lite)]
    public class Cart66LiteIsFBACondition : GenericModuleIsFBACondition
    { }
}
