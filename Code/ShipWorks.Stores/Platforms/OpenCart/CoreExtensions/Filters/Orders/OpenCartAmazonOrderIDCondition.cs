using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OpenCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("OpenCart Amazon Order ID", "OpenCart.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.OpenCart)]
    public class OpenCartAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
