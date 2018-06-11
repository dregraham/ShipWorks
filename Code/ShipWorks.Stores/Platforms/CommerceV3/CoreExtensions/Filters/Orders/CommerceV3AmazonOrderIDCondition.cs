using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CommerceV3.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("CommerceV3 Amazon Order ID", "CommerceV3.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.CommerceV3)]
    public class CommerceV3AmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    {

    }
}