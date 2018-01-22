using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Jigoshop.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Jigoshop Is Fulfilled By Amazon", "Jigoshop.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Jigoshop)]
    public class JigoshopIsFBACondition : GenericModuleIsFBACondition
    { }
}
