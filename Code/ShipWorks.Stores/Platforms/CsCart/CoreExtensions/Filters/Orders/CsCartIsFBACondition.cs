using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CsCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("CsCart Is Fulfilled By Amazon", "CsCart.IsFBA")]
    [ConditionStoreType(StoreTypeCode.CsCart)]
    public class CsCartIsFBACondition : GenericModuleIsFBACondition
    { }
}
