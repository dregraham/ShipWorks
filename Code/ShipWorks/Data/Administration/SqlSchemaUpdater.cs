using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Transactions;
using Autofac;
using Interapptive.Shared.Data;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using NDesk.Options;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.CommandLineOptions;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration.VersionSpecificUpdates;
using ShipWorks.Data.Connection;
using ShipWorks.Filters;
using ShipWorks.Users.Audit;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Manages upgrading the database schema
    /// </summary>
    public static class SqlSchemaUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlSchemaUpdater));

        // Used for executing scripts
        static SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Res.Data.Administration.Scripts.Update");

        // Used for loading \ executing support sql
        private static readonly SqlScriptLoader supportSqlLoader = new SqlScriptLoader("ShipWorks.Res.Data.Administration.Scripts.Support");

        /// <summary>
        /// Get the database schema version that is required by this version of ShipWorks
        /// </summary>
        public static Version GetRequiredSchemaVersion()
        {
            return GetUpdateScripts().Last().SchemaVersion;
        }

        /// <summary>
        /// Indicates if the connection to the given database is the exact database version required for ShipWorks
        /// </summary>
        public static bool IsCorrectSchemaVersion()
        {
            return GetInstalledSchemaVersion() == GetRequiredSchemaVersion() &&
                GetInstalledAssemblyVersion() == GetRequiredSchemaVersion();
        }

        /// <summary>
        /// Gets whether an update is required
        /// </summary>
        /// <returns></returns>
        public static bool IsUpgradeRequired()
        {
            return GetInstalledSchemaVersion() < GetRequiredSchemaVersion() ||
                   GetInstalledAssemblyVersion() < GetRequiredSchemaVersion();
        }

        /// <summary>
        /// Get the version of the installed assembly
        /// </summary>
        private static Version GetInstalledAssemblyVersion() =>
            GetVersionFromStoredProcedure("GetAssemblySchemaVersion");

        /// <summary>
        /// Get the version from GetBuildVersion in the database
        /// </summary>
        /// <returns></returns>
        public static Version GetBuildVersion() =>
            GetVersionFromStoredProcedure("GetBuildVersion");

        /// <summary>
        /// Execute the given stored procedure and return a version
        /// </summary>
        private static Version GetVersionFromStoredProcedure(string procedureName)
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                try
                {
                    using (DbCommand cmd = DbCommandProvider.Create(con, procedureName))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        return new Version((string) DbCommandProvider.ExecuteScalar(cmd));
                    }
                }
                catch (SqlException ex)
                {
                    // "Could not find stored procedure"
                    if (ex.Number == 2812 || ex.Number == 21343)
                    {
                        return new Version(3, 0);
                    }

                    throw;
                }
                catch (ArgumentException ex)
                {
                    // We can't figure out the version, which means it's been modified
                    if (ex.Message.Contains("Version"))
                    {
                        throw new InvalidShipWorksDatabaseException("Invalid ShipWorks database.", ex);
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Get the schema version of the ShipWorks database
        /// </summary>
        public static Version GetInstalledSchemaVersion()
        {
            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    try
                    {
                        return GetInstalledSchemaVersion(con);
                    }
                    catch (SqlException ex)
                    {
                        // "Could not find stored procedure"
                        if (ex.Number == 2812 || ex.Number == 21343)
                        {
                            throw new InvalidShipWorksDatabaseException("Invalid ShipWorks database.", ex);
                        }

                        throw;
                    }
                    catch (ArgumentException ex)
                    {
                        // We can't figure out the version, which means it's been modified
                        if (ex.Message.Contains("Version"))
                        {
                            throw new InvalidShipWorksDatabaseException("Invalid ShipWorks database.", ex);
                        }

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Create progress items
        /// </summary>
        private static (ProgressItem progressScripts, ProgressItem progressFunctionality, ProgressItem progressFilterCounts) CreateProgressItems(IProgressProvider progressProvider)
        {
            // Create the primary progress item
            ProgressItem progressScripts = new ProgressItem("Update Tables");
            progressScripts.CanCancel = false;
            progressProvider.ProgressItems.Add(progressScripts);

            // Create the functionality item
            ProgressItem progressFunctionality = new ProgressItem("Update Functionality");
            progressFunctionality.CanCancel = false;
            progressProvider.ProgressItems.Add(progressFunctionality);

            // Create the functionality item
            ProgressItem progressFilterCounts = new ProgressItem("Calculate Initial Filter Counts");
            progressFilterCounts.CanCancel = false;
            progressProvider.ProgressItems.Add(progressFilterCounts);

            return (progressScripts, progressFunctionality, progressFilterCounts);
        }

        /// <summary>
        /// Upgrade the current database to the latest version.  debuggingMode is only provided as an option for debugging purposes, and should always be false in
        /// customer or production scenarios.
        /// </summary>
        public static void UpdateDatabase(IProgressProvider progressProvider, TelemetricResult<Unit> telemetryEvent, bool debuggingMode = false)
        {
            Version installedSchema = GetInstalledSchemaVersion();
            Version installedAssembly = GetInstalledAssemblyVersion();

            log.Info($"Upgrading database to {GetRequiredSchemaVersion()}. From Schema: {installedSchema} & Assembly: {installedAssembly}.");

            // Create progress items
            var progressItems = CreateProgressItems(progressProvider);
            ProgressItem progressScripts = progressItems.progressScripts;
            ProgressItem progressFunctionality = progressItems.progressFunctionality;
            ProgressItem progressFilterCounts = progressItems.progressFilterCounts;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ISqlUtility sqlUtility = lifetimeScope.Resolve<ISqlUtility>();

                // Put the SuperUser in scope, and don't audit
                using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
                {
                    SqlSession.Current.Configuration.ForceWorkstationID = true;

                    using (new ExistingConnectionScope())
                    {
                        // Start by disconnecting all users. Allow for a long timeout while trying to regain a connection when in single user mode
                        // because reconnection to a very large database seems to take some time after running a big upgrade
                        using (SingleUserModeScope singleUserScope = debuggingMode ?
                            null :
                            new SingleUserModeScope(ExistingConnectionScope.ScopedConnection, TimeSpan.FromMinutes(1), sqlUtility))
                        {
                            try
                            {
                                {
                                    using (new OrderArchiveUpgradeDatabaseScope(ExistingConnectionScope.ScopedConnection))
                                    {
                                        // Update the tables
                                        UpdateScripts(installedSchema, progressScripts, telemetryEvent);

                                        // Functionality starting
                                        progressFunctionality.Starting();

                                        // Update the assemblies
                                        UpdateAssemblies(progressFunctionality);

                                        // If the filter sql version has changed, that means we need to regenerate them to get updated calculation SQL into the database
                                        UpdateFilters(progressFunctionality, progressFilterCounts, ExistingConnectionScope.ScopedConnection, ExistingConnectionScope.ScopedTransaction);

                                        // Try to restore multi-user mode with the existing connection, since re-acquiring a connection after a large
                                        // database upgrade can take time and cause a timeout.
                                        if (singleUserScope != null)
                                        {
                                            SingleUserModeScope.RestoreMultiUserMode();
                                        }

                                        ApplyVersionSpecificUpdates(installedAssembly);

                                        // Execute any support scripts
                                        ExecuteSupportSql(ExistingConnectionScope.ScopedConnection);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("UpdateDatabase failed", ex);
                                throw;
                            }
                            finally
                            {
                                SqlSession.Current.Configuration.ForceWorkstationID = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Apply version specific updates
        /// </summary>
        private static void ApplyVersionSpecificUpdates(Version installed)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                // Commenting out filter until we know which version specific updates have successfully been run
                IEnumerable<IVersionSpecificUpdate> applicableUpdates =
                    lifetimeScope.Resolve<IEnumerable<IVersionSpecificUpdate>>()
                        .Where(x => installed < x.AppliesTo || x.AlwaysRun)
                        .OrderBy(x => x.AppliesTo);

                foreach (IVersionSpecificUpdate versionSpecificUpdate in applicableUpdates)
                {
                    log.Info($"Applying versionSepcificUpdate for version {versionSpecificUpdate.AppliesTo}");
                    versionSpecificUpdate.Update();
                    log.Info($"Applied versionSepcificUpdate for version {versionSpecificUpdate.AppliesTo}");
                }
            }
        }

        /// <summary>
        /// Get a list of all the update scripts in ShipWorks, ordered from smallest version to greatest.
        /// </summary>
        public static List<SqlUpdateScript> GetUpdateScripts()
        {
            List<SqlUpdateScript> scripts = new List<SqlUpdateScript>();

            foreach (string resource in sqlLoader.ScriptResources)
            {
                scripts.Add(new SqlUpdateScript(resource));
            }

            return scripts.OrderBy(s => s.SchemaVersion).ToList();
        }

        /// <summary>
        /// Update the schema version store procedure to match the required schema version.  This should only be called after installing or updating to the latest schema.
        /// </summary>
        public static void UpdateSchemaVersionStoredProcedure(DbConnection con)
        {
            UpdateSchemaVersionStoredProcedure(con, GetRequiredSchemaVersion());
        }

        /// <summary>
        /// Update the schema version stored procedure to say the current schema is the given version
        /// </summary>
        private static void UpdateSchemaVersionStoredProcedure(DbConnection con, Version version)
        {
            using (DbCommand cmd = DbCommandProvider.Create(con))
            {
                UpdateSchemaVersionStoredProcedure(cmd, version);
            }
        }

        /// <summary>
        /// Update the schema version stored procedure to say the current schema is the given version
        /// </summary>
        private static void UpdateSchemaVersionStoredProcedure(DbCommand cmd, Version version)
        {
            UpdateVersionStoredProcedure(cmd, version, "GetSchemaVersion", true);
        }

        /// <summary>
        /// Update the assembly version stored procedure to say the current schema is the given version
        /// </summary>
        public static void UpdateAssemblyVersionStoredProcedure(DbCommand cmd)
        {
            UpdateVersionStoredProcedure(cmd, GetRequiredSchemaVersion(), "GetAssemblySchemaVersion", true);
        }

        /// <summary>
        /// Update the stored procedure that specifies the schema version to which the installed assembly applies
        /// </summary>
        public static void UpdateBuildVersionProcedure(DbCommand cmd)
        {
            Version assemblyVersion = typeof(SqlSchemaUpdater).Assembly.GetName().Version;
            UpdateVersionStoredProcedure(cmd, assemblyVersion, "GetBuildVersion", false);
        }

        /// <summary>
        /// Update a stored procedure for checking a version
        /// </summary>
        private static void UpdateVersionStoredProcedure(DbCommand cmd, Version version, string procedureName, bool trimVersion)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }

            cmd.CommandText = string.Format(@"
                IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    DROP PROCEDURE [dbo].[{0}]", procedureName);
            DbCommandProvider.ExecuteNonQuery(cmd);

#if DEBUG
            string withEncryption = "";
#else
                string withEncryption = "WITH ENCRYPTION";
#endif

            string versionString = trimVersion ? version.ToString(4) : version.ToString();

            cmd.CommandText = string.Format(@"
                CREATE PROCEDURE dbo.{2}
                {0}
                AS
                SELECT '{1}' AS 'SchemaVersion'", withEncryption, versionString, procedureName);
            DbCommandProvider.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Upgrade a 3.x database to the current version.
        /// </summary>
        private static void UpdateScripts(Version installed, ProgressItem progress, TelemetricResult<Unit> telemetryEvent)
        {
            progress.Starting();
            progress.Detail = "Preparing...";

            // Get all the update scripts
            List<SqlUpdateScript> updateScripts = GetUpdateScripts().Where(s => s.SchemaVersion > installed).ToList();

            // Start with generic progress message
            progress.Detail = "Updating...";

            // Store the event handler in a variable so it can be removed easily later
            SqlInfoMessageEventHandler infoMessageHandler = CreateInfoMessageHandler(progress);

            // Listen for script messages to display to the user
            SqlConnection sqlConn = ExistingConnectionScope.ScopedConnection.AsSqlConnection();
            if (sqlConn != null)
            {
                sqlConn.InfoMessage += infoMessageHandler;
            }

            // Determine the percent-value of each update script
            double scriptProgressValue = 100.0 / (double) updateScripts.Count;

            // Go through each update script (they are already sorted in version order)
            foreach (SqlUpdateScript script in updateScripts)
            {
                telemetryEvent.RunTimedEvent($"ExecutionTimeInMilliseconds.{script.SchemaVersion}",
                                             () => ExecuteUpdateScript(progress, updateScripts, scriptProgressValue, script));
            }

            // Since we have a single, long-lived connection, we want to remove the message handler so future messages
            // get handled normally
            if (sqlConn != null)
            {
                sqlConn.InfoMessage -= infoMessageHandler;
            }

            progress.PercentComplete = 100;
            progress.Detail = "Done";
            progress.Completed();
        }

        /// <summary>
        /// Execute an individual update script
        /// </summary>
        private static void ExecuteUpdateScript(ProgressItem progress, List<SqlUpdateScript> updateScripts, double scriptProgressValue, SqlUpdateScript script)
        {
            ExistingConnectionScope.BeginTransaction();

            log.InfoFormat("Updating to {0}", script.SchemaVersion);

            // How many scripts we've finished with so far
            int scriptsCompleted = updateScripts.IndexOf(script);

            // Execute the script
            ISqlScript executor = sqlLoader[script.ScriptName];

            // Update the progress as we complete each batch in the script
            executor.BatchCompleted += delegate (object sender, SqlScriptBatchCompletedEventArgs args)
            {
                // Update the progress
                progress.PercentComplete = Math.Min(100, ((int) (scriptsCompleted * scriptProgressValue)) + (int) ((args.Batch + 1) * (scriptProgressValue / executor.Batches.Count)));
            };

            // Run all the batches in the script
            ExistingConnectionScope.ExecuteWithCommand(executor.Execute);
            ExistingConnectionScope.ExecuteWithCommand(x => UpdateSchemaVersionStoredProcedure(x, script.SchemaVersion));
            ExistingConnectionScope.Commit();
        }

        /// <summary>
        /// Create a SQL info message handler
        /// </summary>
        private static SqlInfoMessageEventHandler CreateInfoMessageHandler(ProgressItem progress)
        {
            return (sender, e) =>
            {
                if (progress.Status == ProgressItemStatus.Running && !e.Message.StartsWith("Caution"))
                {
                    progress.Detail = e.Message;
                }
            };
        }

        /// <summary>
        /// Deploy the latest 3x assemblies
        /// </summary>
        private static void UpdateAssemblies(ProgressItem progress)
        {
            progress.Detail = "Deploying assemblies...";

            SqlAssemblyDeployer.DeployAssemblies(ExistingConnectionScope.ScopedConnection, ExistingConnectionScope.ScopedTransaction);
        }

        /// <summary>
        /// Since our filters use column masks that are exactly dependent on column positioning, we regenerate them every time
        /// there is a schema change, just in case.
        /// </summary>
        private static void UpdateFilters(ProgressItem progressFunctionality, ProgressItem progressFilterCounts, DbConnection connection, DbTransaction transaction)
        {
            progressFunctionality.Detail = "Updating filters...";

            try
            {
                // We need to push a new scope for the layout context, b\c if the user ends up canceling the wizard, it needs to be restored to the
                // way it was.  And if it doesn't, the layout context gets reloaded anyway.
                FilterLayoutContext.PushScope();

                // Regenerate the filters
                ExistingConnectionScope.ExecuteWithAdapter(FilterLayoutContext.Current.RegenerateAllFilters);

                // We can wipe any dirties and any current checkpoint - they don't matter since we have regenerated all filters anyway
                SqlUtility.TruncateTable("FilterNodeContentDirty", connection, transaction);
                SqlUtility.TruncateTable("FilterNodeUpdateCheckpoint", connection, transaction);

                // Functionality is done
                progressFunctionality.PercentComplete = 100;
                progressFunctionality.Detail = "Done";
                progressFunctionality.Completed();

                using (new LoggedStopwatch(log, "Calculate initial filter counts during database upgrade."))
                {
                    using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                    {
                        IFilterHelper filterHelper = scope.Resolve<IFilterHelper>();
                        filterHelper.CalculateInitialFilterCounts(connection, progressFilterCounts, 0);
                    }
                }
            }
            finally
            {
                FilterLayoutContext.PopScope();
            }
        }

        /// <summary>
        /// Get the schema version of the ShipWorks database on the given connection
        /// </summary>
        public static Version GetInstalledSchemaVersion(DbConnection con)
        {
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "GetSchemaVersion";
            cmd.CommandType = CommandType.StoredProcedure;

            return new Version((string) DbCommandProvider.ExecuteScalar(cmd));
        }

        /// <summary>
        /// Handle command line requests
        /// </summary>
        private class CommandLineHandler : ICommandLineCommandHandler
        {
            /// <summary>
            /// Name of the command
            /// </summary>
            public string CommandName
            {
                get { return "getdbschemaversion"; }
            }

            /// <summary>
            /// Run the command with the given arguments
            /// </summary>
            public Task Execute(List<string> args)
            {
                string type = null;

                // Need to extract the type
                OptionSet optionSet = new OptionSet()
                    {
                        { "t|type=", v =>  type = v  },
                        { "<>", v => { throw new CommandLineCommandArgumentException(CommandName, v, "Invalid arguments passed to command."); } }
                    };

                optionSet.Parse(args);

                if (string.IsNullOrEmpty(type))
                {
                    throw new CommandLineCommandArgumentException(CommandName, "type", "The required 'type' parameter was not specified.");
                }

                switch (type)
                {
                    case "required":
                        {
                            // To make things easy we return the result in the ExitCode.  This means we are restricted to integers. So we build
                            // a new int from the schema id
                            int schemaID = GetSchemaID(GetRequiredSchemaVersion());

                            log.InfoFormat("Required schema version: {0}", schemaID);
                            Environment.ExitCode = schemaID;

                            break;
                        }

                    case "database":
                        {
                            // At the point in which this is called, SqlSession has not been setup
                            SqlSession.Initialize();

                            try
                            {
                                if (SqlSession.IsConfigured && SqlSession.Current.CanConnect())
                                {
                                    // To make things easy we return the result in the ExitCode.  This means we are restricted to integers. So we build
                                    // a new int from the schema id
                                    int schemaID = GetSchemaID(GetInstalledSchemaVersion());

                                    log.InfoFormat("Database schema version  {0}", schemaID);
                                    Environment.ExitCode = schemaID;
                                }
                                else
                                {
                                    log.Warn("Could not determine database schema ID since SqlSession is not configured.");

                                    // We don't know
                                    Environment.ExitCode = 0;
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("Could not determine database schema ID", ex);
                                Environment.ExitCode = 0;
                            }

                            break;
                        }

                    default:
                        {
                            throw new CommandLineCommandArgumentException(CommandName, "type", string.Format("Invalid value passed to 'type' parameter: {0}", type));
                        }
                }

                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Get the SchemaID representation of the given version number
        /// </summary>
        private static int GetSchemaID(Version version)
        {
            return
                (version.Major << 24) +
                (version.Minor << 16) +
                (version.Build << 8) +
                (version.Revision);
        }

        /// <summary>
        /// Execute any support SQL scripts
        /// </summary>
        private static void ExecuteSupportSql(DbConnection con)
        {
            try
            {
                foreach (string scriptName in supportSqlLoader.ScriptResources)
                {
                    supportSqlLoader[scriptName].Execute(con);
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while adding support sql scripts.", ex);
                throw;
            }
        }
    }
}
