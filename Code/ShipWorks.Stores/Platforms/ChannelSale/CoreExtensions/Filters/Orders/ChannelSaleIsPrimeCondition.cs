using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ChannelSale.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("ChannelSale Is Amazon Prime", "ChannelSale.IsPrime")]
    [ConditionStoreType(StoreTypeCode.ChannelSale)]
    public class ChannelSaleIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
