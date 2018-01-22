using ShipWorks.Filters.Content;
using ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Filters.Orders;

namespace ShipWorks.Stores.Platforms.StageBloc.CoreExtensions.Filters.Orders
{
    /// <summary>
    /// Filter condition for Amazon Order ID
    /// </summary>
    [ConditionElement("StageBloc Amazon Order ID", "StageBloc.AmazonOrderID")]
    [ConditionStoreType(StoreTypeCode.StageBloc)]
    public class StageBlocAmazonOrderIDCondition : GenericModuleAmazonOrderIDCondition
    { }
}
