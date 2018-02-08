using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.RevolutionParts.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("RevolutionParts Is Amazon Prime", "RevolutionParts.IsPrime")]
    [ConditionStoreType(StoreTypeCode.RevolutionParts)]
    public class RevolutionPartsIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
