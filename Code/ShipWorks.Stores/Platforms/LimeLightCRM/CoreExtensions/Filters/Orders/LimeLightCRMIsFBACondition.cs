using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LimeLightCRM.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("LimeLightCRM Is Fulfilled By Amazon", "LimeLightCRM.IsFBA")]
    [ConditionStoreType(StoreTypeCode.LimeLightCRM)]
    public class LimeLightCRMIsFBACondition : GenericModuleIsFBACondition
    { }
}
