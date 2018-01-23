using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.RevolutionParts.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("RevolutionParts Is Fulfilled By Amazon", "RevolutionParts.IsFBA")]
    [ConditionStoreType(StoreTypeCode.RevolutionParts)]
    public class RevolutionPartsIsFBACondition : GenericModuleIsFBACondition
    { }
}
