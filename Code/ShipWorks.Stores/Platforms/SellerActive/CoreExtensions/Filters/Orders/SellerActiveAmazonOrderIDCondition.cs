using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerActive.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("SellerActive Amazon Order ID", "SellerActive.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.SellerActive)]
    public class SellerActiveAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
