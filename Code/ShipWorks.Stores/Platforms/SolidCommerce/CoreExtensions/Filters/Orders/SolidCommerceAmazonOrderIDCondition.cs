using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SolidCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("SolidCommerce Amazon Order ID", "SolidCommerce.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.SolidCommerce)]
    public class SolidCommerceAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
