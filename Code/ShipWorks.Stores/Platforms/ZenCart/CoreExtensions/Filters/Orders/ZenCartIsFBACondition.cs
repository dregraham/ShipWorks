using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ZenCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("ZenCart Is Fulfilled By Amazon", "ZenCart.IsFBA")]
    [ConditionStoreType(StoreTypeCode.ZenCart)]
    public class ZenCartIsFBACondition : GenericModuleIsFBACondition
    { }
}
