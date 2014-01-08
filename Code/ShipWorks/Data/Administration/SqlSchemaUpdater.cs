using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Interapptive.Shared.Data;
using NDesk.Options;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration.UpdateFrom2x.Database;
using ShipWorks.Data.Connection;
using ShipWorks.Filters;
using ShipWorks.Users.Audit;
using log4net;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Manages upgrading the database schema
    /// </summary>
    public static class SqlSchemaUpdater
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlSchemaUpdater));

        private static SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Data.Administration.Scripts.Update");

        /// <summary>
        /// Get the dabase schema version that is required by this version of ShipWorks
        /// </summary>
        public static string GetRequiredSchemaVersion()
        {
            return (new UpdateScriptManager()).GetRequiredSchemaVersion();
        }

        /// <summary>
        /// Indiciates if the connection to the given database is the exact database version required for ShipWorks
        /// </summary>
        public static bool IsCorrectSchemaVersion()
        {
            return GetInstalledSchemaVersion() == GetRequiredSchemaVersion();
        }

        /// <summary>
        /// Determines whether [is version less than three].
        /// </summary>
        /// <returns></returns>
        public static bool IsVersionLessThanThree()
        {
            return IsVersionLessThanThree(GetInstalledSchemaVersion());
        }

        /// <summary>
        /// Determines whether the specified installed version is less than 3 (likely 2)
        /// </summary>
        /// <param name="installedVersion">The installed version.</param>
        /// <returns></returns>
        public static bool IsVersionLessThanThree(string installedVersion)
        {
            Version installed;

            if (!Version.TryParse(installedVersion, out installed))
            {
                // Schema is no longer using a standard version format. This happened around v3.8.
                return false;
            }

            return installed.Major < 3;
        }

        /// <summary>
        /// Determines whether the installed version is less than 1.2.x [the specified installed version].
        /// </summary>
        /// <param name="installedVersion">The installed version.</param>
        /// <returns></returns>
        public static bool IsVersionLessThanOneTwo(string installedVersion)
        {
            if (!IsVersionLessThanThree(installedVersion))
            {
                return false;
            }

            return Version.Parse(installedVersion) < new Version(1, 2);
        }

        /// <summary>
        /// Get the schema version of the ShipWorks database
        /// </summary>
        public static string GetInstalledSchemaVersion()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
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
            }
        }

        /// <summary>
        /// Upgrade the current database to the latest version.  debuggingMode is only provided as an option for debugging purposes, and should always be false in 
        /// customer or production scenarios.
        /// </summary>
        public static void UpdateDatabase(ProgressProvider progressProvider, bool debuggingMode = false)
        {
            string installed = GetInstalledSchemaVersion();

            log.InfoFormat("Upgrading database from {0} to {1}", installed, GetRequiredSchemaVersion());

            // Create the primary progress item
            ProgressItem progressScripts = new ProgressItem("Update Tables");
            progressScripts.CanCancel = false;
            progressProvider.ProgressItems.Add(progressScripts);

            // Create the functionality item
            ProgressItem progressFunctionality = new ProgressItem("Update Functionality");
            progressFunctionality.CanCancel = false;
            progressProvider.ProgressItems.Add(progressFunctionality);

            // Start by disconnecting all users.
            using (SingleUserModeScope singleUserScope = debuggingMode ? null : new SingleUserModeScope())
            {
                try
                {
                    // Put the SuperUser in scope, and don't audit
                    using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
                    {
                        using (TransactionScope transaction = new TransactionScope(debuggingMode ? TransactionScopeOption.Suppress : TransactionScopeOption.Required, TimeSpan.FromMinutes(20)))
                        {
                            // Update the tables
                            UpdateScripts(installed, progressScripts);

                            // Functionality starting
                            progressFunctionality.Starting();

                            // Update the assemblies
                            UpdateAssemblies(progressFunctionality);

                            // We could be running in the middle of a 2x migration, in which case there are no filters yet and certain other things.
                            // So the following stuff only runs when we are in a "regular" 3x update.
                            if (!MigrationController.IsMigrationInProgress())
                            {
                                // If the filter sql version has changed, that means we need to regenerate them to get updated calculation SQL into the database
                                UpdateFilters(progressFunctionality);
                            }

                            // Now we need to update the database to return the correct schema version that is now installed
                            using (SqlConnection con = SqlSession.Current.OpenConnection())
                            {
                                UpdateSchemaVersionStoredProcedure(con);
                            }

                            // Functionality is done
                            progressFunctionality.PercentComplete = 100;
                            progressFunctionality.Detail = "Done";
                            progressFunctionality.Completed();

                            transaction.Complete();
                        }
                    }

                    // Clear out the pool so any connection holding onto SINGLE_USER gets released
                    SqlConnection.ClearAllPools();

                    FileGrow(installed);

                    // This was needed for databases created before Beta6.  Any ALTER DATABASE statements must happen outside of transaction, so we had to put this here (and do it everytime, even if not needed)
                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        SqlUtility.SetChangeTrackingRetention(con, 1);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("UpdateDatabase failed", ex);

                    throw;
                }
            }
        }

        /// <summary>
        /// If we were upgrading from 3.1.21 or before we adjust the FILEGROW settings.  Can't be in a transaction, so has to be here.
        /// </summary>
        private static void FileGrow(string installed)
        {
            Version parsedVersion;

            if (Version.TryParse(installed, out parsedVersion) && parsedVersion < new Version(3, 1, 21, 0))
            {
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = @"

                                DECLARE @dbName nvarchar(100)
                                DECLARE @dataName nvarchar(100)
                                DECLARE @logName nvarchar(100)

                                SET @dbName = DB_NAME()
                                SELECT @dataName = name FROM sys.database_files WHERE type = 0
                                SELECT @logName = name FROM sys.database_files WHERE type = 1

                                EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @dataName + ''', FILEGROWTH = 100MB)' )
                                EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @logName + ''', FILEGROWTH = 100MB)' )";

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Update the schema version store procuedure to match the required schema version.  This should only be called after installing or updating to the latest schema.
        /// </summary>
        public static void UpdateSchemaVersionStoredProcedure(SqlConnection con)
        {
            UpdateSchemaVersionStoredProcedure(con, GetRequiredSchemaVersion());
        }

        /// <summary>
        /// Update the schema version stored procedure to say the current schema is the given version
        /// </summary>
        public static void UpdateSchemaVersionStoredProcedure(SqlConnection con, string version)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }

            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = @"
                IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetSchemaVersion]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    DROP PROCEDURE [dbo].[GetSchemaVersion]";
                SqlCommandProvider.ExecuteNonQuery(cmd);

#if DEBUG
                string withEncryption = "";
#else
                string withEncryption = "WITH ENCRYPTION";
#endif

                cmd.CommandText = string.Format(@"
                CREATE PROCEDURE dbo.GetSchemaVersion 
                {0}
                AS 
                SELECT '{1}' AS 'SchemaVersion'", withEncryption, version);
                SqlCommandProvider.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Upgrade a 3.x database to the current version.
        /// </summary>
        private static void UpdateScripts(string installed, ProgressItem progress)
        {
            progress.Starting();
            progress.Detail = "Preparing...";

            UpdateScriptManager updateScriptManager = new UpdateScriptManager();

            // Get all the update scripts
            List<SqlUpdateScript> updateScripts = updateScriptManager.GetUpdateScripts(installed).ToList();

            // Start with generic progress msg
            progress.Detail = "Updating...";

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // Listen for script messages to display to the user
                con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                {
                    if (progress.Status == ProgressItemStatus.Running)
                    {
                        if (!e.Message.StartsWith("Caution", StringComparison.OrdinalIgnoreCase))
                        {
                            progress.Detail = e.Message;
                        }
                    }
                };

                // Determine the percent-value of each update script
                double scriptProgressValue = 100.0/(double)updateScripts.Count;

                // Go through each update script (they are already sorted in version order)
                foreach (SqlUpdateScript script in updateScripts)
                {
                    log.InfoFormat("Updating to {0}", script.SchemaVersion);

                    // How many scripts we've finished with so far
                    int scriptsCompleted = updateScripts.IndexOf(script);

                    // Execute the script
                    SqlScript executor = sqlLoader[script.ScriptName];

                    // Update the progress
                    progress.PercentComplete = Math.Min(100, (int)(scriptProgressValue*scriptsCompleted));

                    // Run all the batches in the script
                    executor.Execute(con);
                }

                progress.PercentComplete = 100;
                progress.Detail = "Done";
                progress.Completed();
            }
        }

        /// <summary>
        /// Deploy the latest 3x assemblies
        /// </summary>
        private static void UpdateAssemblies(ProgressItem progress)
        {
            progress.Detail = "Deploying assemblies...";

            // Update the SQL Assemblies
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlAssemblyDeployer.DeployAssemblies(con);
            }
        }

        /// <summary>
        /// Since our filters use column masks that are exactly dependant on column positioning, we regenerate them every time
        /// there is a schema change, just in case.
        /// </summary>
        private static void UpdateFilters(ProgressItem progress)
        {
            progress.Detail = "Updating filters...";

            try
            {
                // We need to push a new scope for the layout context, b\c if the user ends up cancelling the wizard, it needs to be restored to the
                // way it was.  And if it doesnt, the layout context gets reloaded anyway.
                FilterLayoutContext.PushScope();

                // Regenerate the filters
                FilterLayoutContext.Current.RegenerateAllFilters(SqlAdapter.Default);

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
        }

        /// <summary>
        /// Get the schema version of the ShipWorks database on the given connection
        /// </summary>
        public static string GetInstalledSchemaVersion(SqlConnection con)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "GetSchemaVersion";
            cmd.CommandType = CommandType.StoredProcedure;

            return (string)SqlCommandProvider.ExecuteScalar(cmd);
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
                get
                {
                    return "checkneedsupgrade";
                }
            }

            /// <summary>
            /// Run the command with the given arguments
            /// </summary>
            public void Execute(List<string> args)
            {
                string swSchema = null;

                // Need to extract the type
                OptionSet optionSet = new OptionSet()
                {
                    { "dbschema=", s => swSchema = s },
                    { "<>", v => { throw new CommandLineCommandArgumentException(CommandName, v, "Invalid arguments passed to command."); } }
                };

                optionSet.Parse(args);

                // At the point in which this is called, SqlSession has not been setup
                SqlSession.Initialize();

                try
                {
                    if (SqlSession.IsConfigured && SqlSession.Current.CanConnect())
                    {
                        // To make things easy we return the result in the ExitCode.  This means we are restricted to integers. So we build
                        // a new int from the schema id

                        string installedSchemaVersion = GetInstalledSchemaVersion();

                        UpdateScriptManager scriptManager = new UpdateScriptManager();

                        log.InfoFormat("Database schema version  {0}", installedSchemaVersion);

                        Environment.ExitCode = scriptManager.DoesInstallingVersionRequireDBToBeUpgraded(swSchema, installedSchemaVersion) ? 1 : 0;
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
            }
        }
    }
}