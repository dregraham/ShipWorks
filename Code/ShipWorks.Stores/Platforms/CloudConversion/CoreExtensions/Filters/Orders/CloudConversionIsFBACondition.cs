using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.CloudConversion.CoreExtensions.Filters.Orders
{
    [ConditionElement("CloudConversion Is FBA", "CloudConversion.IsFBA")]
    [ConditionStoreType(StoreTypeCode.CloudConversion)]
    public class CloudConversionIsFBACondition : GenericModuleIsFBACondition
    { }
}