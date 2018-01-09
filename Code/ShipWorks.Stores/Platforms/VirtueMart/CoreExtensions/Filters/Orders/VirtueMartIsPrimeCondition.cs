using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.VirtueMart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("VirtueMart Is Amazon Prime", "VirtueMart.IsPrime")]
    [ConditionStoreType(StoreTypeCode.VirtueMart)]
    public class VirtueMartIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
