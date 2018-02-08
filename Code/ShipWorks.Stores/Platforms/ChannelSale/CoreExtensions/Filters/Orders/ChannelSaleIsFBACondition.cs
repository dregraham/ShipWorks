using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ChannelSale.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("ChannelSale Is Fulfilled By Amazon", "ChannelSale.IsFBA")]
    [ConditionStoreType(StoreTypeCode.ChannelSale)]
    public class ChannelSaleIsFBACondition : GenericModuleIsFBACondition
    { }
}
