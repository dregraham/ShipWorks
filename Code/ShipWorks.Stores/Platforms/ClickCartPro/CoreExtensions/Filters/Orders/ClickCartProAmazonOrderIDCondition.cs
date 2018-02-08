using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ClickCartPro.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("ClickCartPro Amazon Order ID", "ClickCartPro.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.ClickCartPro)]
    public class ClickCartProAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
