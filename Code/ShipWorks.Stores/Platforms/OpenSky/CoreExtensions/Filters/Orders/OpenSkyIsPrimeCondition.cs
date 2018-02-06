using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.OpenSky.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("OpenSky Is Amazon Prime", "OpenSky.IsPrime")]
    [ConditionStoreType(StoreTypeCode.OpenSky)]
    public class OpenSkyIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
