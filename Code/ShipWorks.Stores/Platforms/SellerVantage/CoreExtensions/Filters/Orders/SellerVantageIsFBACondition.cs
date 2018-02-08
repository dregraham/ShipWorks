using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerVantage.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("SellerVantage Is Fulfilled By Amazon", "SellerVantage.IsFBA")]
    [ConditionStoreType(StoreTypeCode.SellerVantage)]
    public class SellerVantageIsFBACondition : GenericModuleIsFBACondition
    { }
}
