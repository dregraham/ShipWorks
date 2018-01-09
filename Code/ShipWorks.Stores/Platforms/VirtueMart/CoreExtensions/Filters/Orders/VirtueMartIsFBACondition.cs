using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.VirtueMart.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("VirtueMart Is Fulfilled By Amazon", "VirtueMart.IsFBA")]
    [ConditionStoreType(StoreTypeCode.VirtueMart)]
    public class VirtueMartIsFBACondition : GenericModuleIsFBACondition
    { }
}
