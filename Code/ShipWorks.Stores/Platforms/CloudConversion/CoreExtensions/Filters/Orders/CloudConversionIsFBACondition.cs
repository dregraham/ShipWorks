using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CloudConversion.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("CloudConversion Is Fulfilled By Amazon", "CloudConversion.IsFBA")]
    [ConditionStoreType(StoreTypeCode.CloudConversion)]
    public class CloudConversionIsFBACondition : GenericModuleIsFBACondition
    { }
}
