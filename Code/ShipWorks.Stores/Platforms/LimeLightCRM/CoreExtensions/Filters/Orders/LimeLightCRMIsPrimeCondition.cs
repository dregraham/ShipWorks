using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LimeLightCRM.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("LimeLightCRM Is Amazon Prime", "LimeLightCRM.IsPrime")]
    [ConditionStoreType(StoreTypeCode.LimeLightCRM)]
    public class LimeLightCRMIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
