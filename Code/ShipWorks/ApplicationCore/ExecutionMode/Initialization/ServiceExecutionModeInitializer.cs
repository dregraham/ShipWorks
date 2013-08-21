using System;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Filters;
using ShipWorks.Filters.Management;

namespace ShipWorks.ApplicationCore.ExecutionMode.Initialization
{
    /// <summary>
    /// Execution mode initializer for background services and windows services
    /// </summary>
    public class ServiceExecutionModeInitializer : ExecutionModeInitializerBase
    {
        /// <summary>
        /// Initialize the ServiceExecutionMode object.  
        /// </summary>
        public override void Initialize()
        {
            PerformCommonInitialization();

            // Register some idle cleanup work.
            DataResourceManager.RegisterResourceCacheCleanup();
            DataPath.RegisterTempFolderCleanup();
            LogSession.RegisterLogCleanup();

            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedFilterCounts", FilterContentManager.DeleteAbandonedFilterCounts, "doing maintenance", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedQuickFilters", QuickFilterHelper.DeleteAbandonedFilters, "doing maintenance", TimeSpan.FromHours(2));
            IdleWatcher.RegisterDatabaseDependentWork("CleanupAbandonedResources", DataResourceManager.DeleteAbandonedResourceData, "cleaning up resources", TimeSpan.FromHours(2));

            // Start idle processing
            IdleWatcher.Initialize();
        }
    }
}
