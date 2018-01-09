using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CloudConversion.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Prime
    /// </summary>
    [ConditionElement("CloudConversion Is Amazon Prime", "CloudConversion.IsPrime")]
    [ConditionStoreType(StoreTypeCode.CloudConversion)]
    public class CloudConversionIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
