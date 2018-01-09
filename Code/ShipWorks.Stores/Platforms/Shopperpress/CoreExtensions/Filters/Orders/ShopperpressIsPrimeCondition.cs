using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Shopperpress.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Shopperpress Is Amazon Prime", "Shopperpress.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Shopperpress)]
    public class ShopperpressIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
