using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Brightpearl.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Brightpearl Is Amazon Prime", "Brightpearl.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Brightpearl)]
    public class BrightpearlIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
