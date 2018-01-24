using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerExpress.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("SellerExpress Amazon Order ID", "SellerExpress.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.SellerExpress)]
    public class SellerExpressAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
