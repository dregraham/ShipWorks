using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Zenventory.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Zenventory Is Amazon Prime", "Zenventory.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Zenventory)]
    public class ZenventoryIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
