using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.Amosoft.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("Amosoft Is Fulfilled By Amazon", "Amosoft.IsFBA")]
    [ConditionStoreType(StoreTypeCode.Amosoft)]
    public class AmosoftIsFBACondition : GenericModuleIsFBACondition
    { }
}
