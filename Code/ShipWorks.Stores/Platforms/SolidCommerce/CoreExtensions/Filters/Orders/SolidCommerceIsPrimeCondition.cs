using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.SolidCommerce.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("SolidCommerce Is Amazon Prime", "SolidCommerce.IsPrime")]
    [ConditionStoreType(StoreTypeCode.SolidCommerce)]
    public class SolidCommerceIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
