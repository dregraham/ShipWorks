using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CloudConversion.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("CloudConversion Amazon Order ID", "CloudConversion.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.CloudConversion)]
    public class CloudConversionAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
