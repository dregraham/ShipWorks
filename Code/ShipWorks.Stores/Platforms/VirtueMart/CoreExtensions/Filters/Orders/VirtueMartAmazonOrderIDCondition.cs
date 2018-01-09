using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.VirtueMart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("VirtueMart Amazon Order ID", "VirtueMart.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.VirtueMart)]
    public class VirtueMartAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
