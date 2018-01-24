using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderBot.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("OrderBot Is Fulfilled By Amazon", "OrderBot.IsFBA")]
    [ConditionStoreType(StoreTypeCode.OrderBot)]
    public class OrderBotIsFBACondition : GenericModuleIsFBACondition
    { }
}
