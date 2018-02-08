using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerCloud.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("SellerCloud Is Fulfilled By Amazon", "SellerCloud.IsFBA")]
    [ConditionStoreType(StoreTypeCode.SellerCloud)]
    public class SellerCloudIsFBACondition : GenericModuleIsFBACondition
    { }
}
