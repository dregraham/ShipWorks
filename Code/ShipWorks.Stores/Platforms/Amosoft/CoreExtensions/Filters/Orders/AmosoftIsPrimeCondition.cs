using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Amosoft.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Amosoft Is Amazon Prime", "Amosoft.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Amosoft)]
    public class AmosoftIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
