using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.InfiPlex.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("InfiPlex Is Fulfilled By Amazon", "InfiPlex.IsFBA")]
    [ConditionStoreType(StoreTypeCode.InfiPlex)]
    public class InfiPlexIsFBACondition : GenericModuleIsFBACondition
    { }
}
