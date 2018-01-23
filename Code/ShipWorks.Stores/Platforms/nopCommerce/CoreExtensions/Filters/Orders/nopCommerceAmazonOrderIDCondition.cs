using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.nopCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("nopCommerce Amazon Order ID", "nopCommerce.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.nopCommerce)]
    public class nopCommerceAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
