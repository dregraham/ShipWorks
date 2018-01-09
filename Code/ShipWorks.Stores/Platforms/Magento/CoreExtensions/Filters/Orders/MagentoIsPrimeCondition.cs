using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Magento.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("Magento Is Amazon Prime", "Magento.IsPrime")]
    [ConditionStoreType(StoreTypeCode.Magento)]
    public class MagentoIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
