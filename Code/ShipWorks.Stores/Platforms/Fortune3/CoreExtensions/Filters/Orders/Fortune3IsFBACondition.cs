using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Fortune3.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Fortune3 Is Fulfilled By Amazon", "Fortune3.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Fortune3)]
    public class Fortune3IsFBACondition : GenericModuleIsFBACondition
    { }
}
