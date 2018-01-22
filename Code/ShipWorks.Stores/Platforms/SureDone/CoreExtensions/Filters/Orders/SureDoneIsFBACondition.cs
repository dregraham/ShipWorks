using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SureDone.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("SureDone Is Fulfilled By Amazon", "SureDone.IsFBA")]
    [ConditionStoreType(StoreTypeCode.SureDone)]
    public class SureDoneIsFBACondition : GenericModuleIsFBACondition
    { }
}
