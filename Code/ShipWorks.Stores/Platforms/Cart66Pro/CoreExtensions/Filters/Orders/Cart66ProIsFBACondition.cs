using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Cart66Pro.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Cart66Pro Is Fulfilled By Amazon", "Cart66Pro.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Cart66Pro)]
    public class Cart66ProIsFBACondition : GenericModuleIsFBACondition
    { }
}
