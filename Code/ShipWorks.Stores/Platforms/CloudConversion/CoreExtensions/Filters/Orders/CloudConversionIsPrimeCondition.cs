using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters;

namespace ShipWorks.Stores.Platforms.CloudConversion.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Prime
    /// </summary>
    [ConditionElement("CloudConversion Is Amazon Prime", "CloudConversion.IsPrime")]
    [ConditionStoreType(StoreTypeCode.GenericModule)]
    public class CloudConversionIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
