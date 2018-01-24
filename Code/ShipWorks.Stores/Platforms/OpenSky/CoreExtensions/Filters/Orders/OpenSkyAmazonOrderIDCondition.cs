using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OpenSky.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("OpenSky Amazon Order ID", "OpenSky.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.OpenSky)]
    public class OpenSkyAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
