using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Shopperpress.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Shopperpress Is Fulfilled By Amazon", "Shopperpress.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Shopperpress)]
    public class ShopperpressIsFBACondition : GenericModuleIsFBACondition
    { }
}
