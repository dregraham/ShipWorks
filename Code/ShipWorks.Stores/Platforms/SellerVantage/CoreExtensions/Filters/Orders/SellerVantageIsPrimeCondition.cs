using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerVantage.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("SellerVantage Is Amazon Prime", "SellerVantage.IsPrime")]
    [ConditionStoreType(StoreTypeCode.SellerVantage)]
    public class SellerVantageIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
