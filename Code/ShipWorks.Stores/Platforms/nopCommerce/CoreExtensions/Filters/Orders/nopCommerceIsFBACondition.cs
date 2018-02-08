using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.nopCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("nopCommerce Is Fulfilled By Amazon", "nopCommerce.IsFBA")]
    [ConditionStoreType(StoreTypeCode.nopCommerce)]
    public class nopCommerceIsFBACondition : GenericModuleIsFBACondition
    { }
}
