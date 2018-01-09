using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.LoadedCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("LoadedCommerce Is Fulfilled By Amazon", "LoadedCommerce.IsFBA")]
    [ConditionStoreType(StoreTypeCode.LoadedCommerce)]
    public class LoadedCommerceIsFBACondition : GenericModuleIsFBACondition
    { }
}
