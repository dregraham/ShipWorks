using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Filters;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option for regenerating all filters.
    /// </summary>
    public class RegenerateAllFiltersCommandLineOption : ICommandLineCommandHandler
    {
        static readonly ILog log = LogManager.GetLogger(typeof(RegenerateAllFiltersCommandLineOption));

        /// <summary>
        /// The CommandName that can be sent to the ShipWorks.exe
        /// </summary>
        public string CommandName
        {
            get { return "regenerateallfilters"; }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Execute(List<string> args)
        {
            try
            {
                log.Info("Processing request to regenerate all filters.");

                try
                {
                    SqlSession.Initialize();

                    DataProvider.InitializeForApplication();

                    UserSession.InitializeForCurrentDatabase();

                    UserManager.InitializeForCurrentUser();
                    UserSession.InitializeForCurrentSession(Program.ExecutionMode);

                    FilterLayoutContext.InitializeForCurrentSession();

                    // We need to push a new scope for the layout context
                    FilterLayoutContext.PushScope();

                    // Regenerate the filters
                    FilterLayoutContext.Current.RegenerateAllFilters(SqlAdapter.Default);

                    // Delete any filter counts we may have abandoned by regenerating
                    FilterContentManager.DeleteAbandonedFilterCounts();

                    // Calling CheckForChanges will fire off the Calculate Initial so that counts get updated.
                    FilterContentManager.CheckForChanges();

                    // We can wipe any dirties and any current checkpoint - they don't matter since we have regenerated all filters anyway
                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        SqlUtility.TruncateTable("FilterNodeContentDirty", con);
                        SqlUtility.TruncateTable("FilterNodeUpdateCheckpoint", con);
                    }
                }
                finally
                {
                    FilterLayoutContext.PopScope();
                }

                log.Info("Successfully redeployed assemblies.");
            }
            catch (SqlException ex)
            {
                log.Error("Failed to regenerate all filters.", ex);
                Environment.ExitCode = ex.ErrorCode;
            }
        }
    }
}