using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Shopp.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Shopp Is Fulfilled By Amazon", "Shopp.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Shopp)]
    public class ShoppIsFBACondition : GenericModuleIsFBACondition
    { }
}
