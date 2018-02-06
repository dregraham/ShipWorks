using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.GeekSeller.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("GeekSeller Is Amazon Prime", "GeekSeller.IsPrime")]
    [ConditionStoreType(StoreTypeCode.GeekSeller)]
    public class GeekSellerIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
