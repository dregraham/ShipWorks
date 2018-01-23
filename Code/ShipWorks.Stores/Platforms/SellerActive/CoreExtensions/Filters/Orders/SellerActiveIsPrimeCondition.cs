using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SellerActive.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("SellerActive Is Amazon Prime", "SellerActive.IsPrime")]
    [ConditionStoreType(StoreTypeCode.SellerActive)]
    public class SellerActiveIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
