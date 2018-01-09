using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.XCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("XCart Amazon Order ID", "XCart.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.XCart)]
    public class XCartAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
