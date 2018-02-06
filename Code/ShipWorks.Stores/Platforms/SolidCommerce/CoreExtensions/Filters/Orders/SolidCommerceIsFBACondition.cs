using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SolidCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("SolidCommerce Is Fulfilled By Amazon", "SolidCommerce.IsFBA")]
    [ConditionStoreType(StoreTypeCode.SolidCommerce)]
    public class SolidCommerceIsFBACondition : GenericModuleIsFBACondition
    { }
}
