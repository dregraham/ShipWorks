using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("CommerceInterface Is Amazon Prime", "CommerceInterface.IsPrime")]
    [ConditionStoreType(StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
