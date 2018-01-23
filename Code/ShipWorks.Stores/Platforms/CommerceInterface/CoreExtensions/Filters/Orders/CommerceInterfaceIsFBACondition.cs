using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("CommerceInterface Is Fulfilled By Amazon", "CommerceInterface.IsFBA")]
    [ConditionStoreType(StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceIsFBACondition : GenericModuleIsFBACondition
    { }
}
