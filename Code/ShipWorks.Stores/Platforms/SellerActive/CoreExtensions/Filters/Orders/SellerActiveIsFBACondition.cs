using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerActive.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("SellerActive Is Fulfilled By Amazon", "SellerActive.IsFBA")]
    [ConditionStoreType(StoreTypeCode.SellerActive)]
    public class SellerActiveIsFBACondition : GenericModuleIsFBACondition
    { }
}
