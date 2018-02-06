using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerExpress.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("SellerExpress Is Fulfilled By Amazon", "SellerExpress.IsFBA")]
    [ConditionStoreType(StoreTypeCode.SellerExpress)]
    public class SellerExpressIsFBACondition : GenericModuleIsFBACondition
    { }
}
