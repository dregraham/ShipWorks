using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("CommerceInterface Amazon Order ID", "CommerceInterface.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
