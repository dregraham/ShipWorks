using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerVantage.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("SellerVantage Amazon Order ID", "SellerVantage.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.SellerVantage)]
    public class SellerVantageAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
