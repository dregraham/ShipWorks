using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Brightpearl.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Brightpearl Is Fulfilled By Amazon", "Brightpearl.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Brightpearl)]
    public class BrightpearlIsFBACondition : GenericModuleIsFBACondition
    { }
}
