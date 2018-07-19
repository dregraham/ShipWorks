﻿using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.TypedViewClasses;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Repository for getting disabled ShipWorks index info and missing index info
    /// </summary>
    public interface IManageDisabledIndexesRepo
    {
        /// <summary>
        /// Get any ShipWorks indexes that are disabled.
        /// </summary>
        ShipWorksDisabledDefaultIndexTypedView GetShipWorksDisabledDefaultIndexesView(ISqlAdapter adapter);

        /// <summary>
        /// Get any missing index requests.
        /// </summary>
        ShipWorksMissingIndexRequestsTypedView GetMissingIndexRequestsView(ISqlAdapter adapter, decimal minIndexUsage);
    }
}
