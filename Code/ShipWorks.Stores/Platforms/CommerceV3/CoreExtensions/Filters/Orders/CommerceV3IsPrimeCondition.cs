using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CommerceV3.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("CommerceV3 is Amazon Prime", "CommerceV3.IsPrime")]
    [ConditionStoreType(StoreTypeCode.CommerceV3)]
    public class CommerceV3IsPrimeCondition : GenericModuleIsPrimeCondition
    {
        
    }
}