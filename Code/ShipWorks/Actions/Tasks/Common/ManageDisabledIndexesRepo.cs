using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.TypedViewClasses;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Repository for getting disabled ShipWorks index info and missing index info
    /// </summary>
    [Component]
    public class ManageDisabledIndexesRepo : IManageDisabledIndexesRepo
    {
        /// <summary>
        /// Get any ShipWorks indexes that are disabled.
        /// </summary>
        public IEnumerable<DisabledIndex> GetShipWorksDisabledDefaultIndexesView(ISqlAdapter adapter)
        {
            ShipWorksDisabledDefaultIndexTypedView disabledDefaultIndexes = new ShipWorksDisabledDefaultIndexTypedView();
            adapter.FetchTypedView(disabledDefaultIndexes);
            return DisabledIndex.FromView(disabledDefaultIndexes);
        }

        /// <summary>
        /// Get any missing index requests that have a usage greater than specified value.
        /// </summary>
        public IEnumerable<MissingIndex> GetMissingIndexRequestsView(ISqlAdapter adapter, decimal minIndexUsage)
        {
            ShipWorksMissingIndexRequestsTypedView missingIndexRequests = new ShipWorksMissingIndexRequestsTypedView();
            RelationPredicateBucket missingIndexRequestsBucket = new RelationPredicateBucket(ShipWorksMissingIndexRequestsFields.IndexAdvantage > minIndexUsage);
            adapter.FetchTypedView(missingIndexRequests, missingIndexRequestsBucket, true);

            return MissingIndex.FromView(missingIndexRequests);
        }
    }
}
