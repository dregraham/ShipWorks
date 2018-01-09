using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LoadedCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("LoadedCommerce Amazon Order ID", "LoadedCommerce.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.LoadedCommerce)]
    public class LoadedCommerceAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
