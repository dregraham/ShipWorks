using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ChannelSale.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("ChannelSale Amazon Order ID", "ChannelSale.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.ChannelSale)]
    public class ChannelSaleAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
