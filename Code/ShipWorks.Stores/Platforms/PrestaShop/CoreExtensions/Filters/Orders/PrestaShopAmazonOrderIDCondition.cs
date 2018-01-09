using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.PrestaShop.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("PrestaShop Amazon Order ID", "PrestaShop.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.PrestaShop)]
    public class PrestaShopAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
