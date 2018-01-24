using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Jigoshop.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Jigoshop Is Amazon Prime", "Jigoshop.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Jigoshop)]
    public class JigoshopIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
