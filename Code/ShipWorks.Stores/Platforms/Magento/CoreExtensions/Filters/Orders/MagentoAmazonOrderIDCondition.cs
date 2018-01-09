using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Magento.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Magento Amazon Order ID", "Magento.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Magento)]
    public class MagentoAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
