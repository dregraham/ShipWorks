using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.osCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("osCommerce Is Fulfilled By Amazon", "osCommerce.IsFBA")]
    [ConditionStoreType(StoreTypeCode.osCommerce)]
    public class osCommerceIsFBACondition : GenericModuleIsFBACondition
    { }
}
