using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerCloud.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("SellerCloud Is Amazon Prime", "SellerCloud.IsPrime")]
    [ConditionStoreType(StoreTypeCode.SellerCloud)]
    public class SellerCloudIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
