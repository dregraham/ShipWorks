using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Magento.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Magento Is Fulfilled By Amazon", "Magento.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Magento)]
    public class MagentoIsFBACondition : GenericModuleIsFBACondition
    { }
}
