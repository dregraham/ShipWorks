using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerExpress.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("SellerExpress Is Amazon Prime", "SellerExpress.IsPrime")]
    [ConditionStoreType(StoreTypeCode.SellerExpress)]
    public class SellerExpressIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
