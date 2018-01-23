using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.ClickCartPro.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("ClickCartPro Is Fulfilled By Amazon", "ClickCartPro.IsFBA")]
    [ConditionStoreType(StoreTypeCode.ClickCartPro)]
    public class ClickCartProIsFBACondition : GenericModuleIsFBACondition
    { }
}
