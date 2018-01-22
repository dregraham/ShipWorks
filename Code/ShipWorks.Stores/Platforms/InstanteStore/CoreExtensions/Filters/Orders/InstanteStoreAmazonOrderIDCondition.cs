using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.InstanteStore.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("InstanteStore Amazon Order ID", "InstanteStore.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.InstaStore)]
    public class InstanteStoreAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
