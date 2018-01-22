using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OpenSky.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("OpenSky Is Fulfilled By Amazon", "OpenSky.IsFBA")]
    [ConditionStoreType(StoreTypeCode.OpenSky)]
    public class OpenSkyIsFBACondition : GenericModuleIsFBACondition
    { }
}
