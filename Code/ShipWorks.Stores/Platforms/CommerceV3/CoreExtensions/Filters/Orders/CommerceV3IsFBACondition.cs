using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CommerceV3.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("CommerceV3 is FulFilled By Amazon", "CommerceV3.IsFBA")]
    [ConditionStoreType(StoreTypeCode.CommerceV3)]
    public class CommerceV3IsFBACondition : GenericModuleIsFBACondition
    {
        
    }
}