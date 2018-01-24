using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.GeekSeller.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("GeekSeller Amazon Order ID", "GeekSeller.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.GeekSeller)]
    public class GeekSellerAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
