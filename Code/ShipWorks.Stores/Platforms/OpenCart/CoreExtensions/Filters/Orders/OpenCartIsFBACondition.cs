using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OpenCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("OpenCart Is Fulfilled By Amazon", "OpenCart.IsFBA")]
    [ConditionStoreType(StoreTypeCode.OpenCart)]
    public class OpenCartIsFBACondition : GenericModuleIsFBACondition
    { }
}
