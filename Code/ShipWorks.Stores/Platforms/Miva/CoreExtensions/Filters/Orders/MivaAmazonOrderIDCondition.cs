using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Miva.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Miva Amazon Order ID", "Miva.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Miva)]
    public class MivaAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
