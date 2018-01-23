using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Zenventory.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Zenventory Is Fulfilled By Amazon", "Zenventory.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Zenventory)]
    public class ZenventoryIsFBACondition : GenericModuleIsFBACondition
    { }
}
