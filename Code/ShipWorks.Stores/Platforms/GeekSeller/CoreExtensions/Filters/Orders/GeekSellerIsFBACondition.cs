using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.GeekSeller.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("GeekSeller Is Fulfilled By Amazon", "GeekSeller.IsFBA")]
    [ConditionStoreType(StoreTypeCode.GeekSeller)]
    public class GeekSellerIsFBACondition : GenericModuleIsFBACondition
    { }
}
