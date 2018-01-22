using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Amosoft.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("Amosoft Amazon Order ID", "Amosoft.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.Amosoft)]
    public class AmosoftAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
