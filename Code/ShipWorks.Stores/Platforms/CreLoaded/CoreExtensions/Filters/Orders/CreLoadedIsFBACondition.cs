using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CreLoaded.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("CreLoaded Is Fulfilled By Amazon", "CreLoaded.IsFBA")]
    [ConditionStoreType(StoreTypeCode.CreLoaded)]
    public class CreLoadedIsFBACondition : GenericModuleIsFBACondition
    { }
}
