using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.StageBloc.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Amazon Prime
    /// </summary>
    [ConditionElement("StageBloc Is Amazon Prime", "StageBloc.IsPrime")]
    [ConditionStoreType(StoreTypeCode.StageBloc)]
    public class StageBlocIsPrimeCondition : GenericModuleIsPrimeCondition
    { }
}
