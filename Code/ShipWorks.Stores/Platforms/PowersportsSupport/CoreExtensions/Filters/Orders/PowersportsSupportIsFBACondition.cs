using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.PowersportsSupport.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("PowersportsSupport Is Fulfilled By Amazon", "PowersportsSupport.IsFBA")]
    [ConditionStoreType(StoreTypeCode.PowersportsSupport)]
    public class PowersportsSupportIsFBACondition : GenericModuleIsFBACondition
    { }
}
