using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderDesk.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("OrderDesk Is Fulfilled By Amazon", "OrderDesk.IsFBA")]
    [ConditionStoreType(StoreTypeCode.OrderDesk)]
    public class OrderDeskIsFBACondition : GenericModuleIsFBACondition
    { }
}
