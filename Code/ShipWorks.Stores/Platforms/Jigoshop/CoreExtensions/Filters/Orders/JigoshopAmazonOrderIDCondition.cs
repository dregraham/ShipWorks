using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Jigoshop.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Jigoshop Amazon Order ID", "Jigoshop.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Jigoshop)]
    public class JigoshopAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
