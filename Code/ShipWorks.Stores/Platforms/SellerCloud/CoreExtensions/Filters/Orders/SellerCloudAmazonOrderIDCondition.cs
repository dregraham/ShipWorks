using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerCloud.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("SellerCloud Amazon Order ID", "SellerCloud.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.SellerCloud)]
    public class SellerCloudAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
