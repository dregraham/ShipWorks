using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.XCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("XCart Is Fulfilled By Amazon", "XCart.IsFBA")]
    [ConditionStoreType(StoreTypeCode.XCart)]
    public class XCartIsFBACondition : GenericModuleIsFBACondition
    { }
}
