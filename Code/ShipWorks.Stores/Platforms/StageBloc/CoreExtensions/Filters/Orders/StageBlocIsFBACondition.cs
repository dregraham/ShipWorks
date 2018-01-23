using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.StageBloc.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Is Fulfilled By Amazon
    /// </summary>
    [ConditionElement("StageBloc Is Fulfilled By Amazon", "StageBloc.IsFBA")]
    [ConditionStoreType(StoreTypeCode.StageBloc)]
    public class StageBlocIsFBACondition : GenericModuleIsFBACondition
    { }
}
