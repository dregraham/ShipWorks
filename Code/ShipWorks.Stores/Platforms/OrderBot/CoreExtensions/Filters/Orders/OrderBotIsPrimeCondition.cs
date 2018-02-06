using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OrderBot.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("OrderBot Is Amazon Prime", "OrderBot.IsPrime")]
    [ConditionStoreType(StoreTypeCode.OrderBot)]
    public class OrderBotIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
