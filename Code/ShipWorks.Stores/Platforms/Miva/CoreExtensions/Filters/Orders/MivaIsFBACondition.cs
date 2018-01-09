using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Miva.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Miva Is Fulfilled By Amazon", "Miva.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Miva)]
    public class MivaIsFBACondition : GenericModuleIsFBACondition
    { }
}
