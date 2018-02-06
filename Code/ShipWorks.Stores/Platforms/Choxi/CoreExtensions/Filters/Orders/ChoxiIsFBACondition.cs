using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Choxi.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Choxi Is Fulfilled By Amazon", "Choxi.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Choxi)]
    public class ChoxiIsFBACondition : GenericModuleIsFBACondition
    { }
}
