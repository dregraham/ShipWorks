using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CsCart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("CsCart Amazon Order ID", "CsCart.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.CsCart)]
    public class CsCartAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
