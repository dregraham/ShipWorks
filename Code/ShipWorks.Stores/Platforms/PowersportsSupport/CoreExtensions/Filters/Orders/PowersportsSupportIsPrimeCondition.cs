using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.PowersportsSupport.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("PowersportsSupport Is Amazon Prime", "PowersportsSupport.IsPrime")]
    [ConditionStoreType(StoreTypeCode.PowersportsSupport)]
    public class PowersportsSupportIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
