using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderDynamics.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("OrderDynamics Is Amazon Prime", "OrderDynamics.IsPrime")]
    [ConditionStoreType(StoreTypeCode.OrderDynamics)]
    public class OrderDynamicsIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
