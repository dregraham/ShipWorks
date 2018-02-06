using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ClickCartPro.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("ClickCartPro Is Amazon Prime", "ClickCartPro.IsPrime")]
    [ConditionStoreType(StoreTypeCode.ClickCartPro)]
    public class ClickCartProIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
