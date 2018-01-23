using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.InstanteStore.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("InstanteStore Is Fulfilled By Amazon", "InstanteStore.IsFBA")]
    [ConditionStoreType(StoreTypeCode.InstaStore)]
    public class InstanteStoreIsFBACondition : GenericModuleIsFBACondition
    { }
}
