using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderDesk.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("OrderDesk Is Amazon Prime", "OrderDesk.IsPrime")]
    [ConditionStoreType(StoreTypeCode.OrderDesk)]
    public class OrderDeskIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
