using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderDynamics.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("OrderDynamics Is Fulfilled By Amazon", "OrderDynamics.IsFBA")]
    [ConditionStoreType(StoreTypeCode.OrderDynamics)]
    public class OrderDynamicsIsFBACondition : GenericModuleIsFBACondition
    { }
}
