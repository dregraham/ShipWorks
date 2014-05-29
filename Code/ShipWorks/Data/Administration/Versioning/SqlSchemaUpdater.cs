using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using Divelements.SandGrid;
using Interapptive.Shared.Data;
using NDesk.Options;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration.Scripts.Update;
using ShipWorks.Data.Administration.UpdateFrom2x.Database;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users.Audit;
using log4net;

namespace ShipWorks.Data.Administration.Versioning
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
        /// Indiciates if the connection to the given database is the exact database version required for ShipWorks
        /// </summary>
        public static bool IsCorrectSchemaVersion()
        {
            return GetDatabaseSchemaVersion().Compare((new SchemaVersionManager()).GetRequiredSchemaVersion()) == SchemaVersionComparisonResult.Equal;
        }

        /// <summary>
        /// Get the schema version of the ShipWorks database
        /// </summary>
        public static SchemaVersion GetDatabaseSchemaVersion()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                try
                {
                    return SqlDatabaseDetail.GetSchemaVersion(con);
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
            SchemaVersion fromVersion = GetDatabaseSchemaVersion();
            SchemaVersion toVersion = (new SchemaVersionManager()).GetRequiredSchemaVersion();

            log.InfoFormat("Upgrading database from {0} to {1}", fromVersion, toVersion);

            // Create the primary progress item
            ProgressItem progressScripts = new ProgressItem("Update Tables");
            progressScripts.CanCancel = false;
            progressProvider.ProgressItems.Add(progressScripts);

            // Create the functionality item
            ProgressItem progressFunctionality = new ProgressItem("Update Functionality");
            progressFunctionality.CanCancel = false;
            progressProvider.ProgressItems.Add(progressFunctionality);

            ProgressItem progressProcesses = new ProgressItem("Programatic Updates");
            progressProcesses.CanCancel = false;
            progressProvider.ProgressItems.Add(progressProcesses);

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
                            SchemaVersionManager schemaVersionManager = new SchemaVersionManager();
                            // Get all the update scripts
                            List<SqlUpdateScript> updateScripts = schemaVersionManager.GetUpdateScripts(fromVersion, toVersion).ToList();


                            // Update the tables
                            UpdateScripts(updateScripts, progressScripts);

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
                                UpdateSchemaVersionStoredProcedure(con, toVersion);
                            }

                            // Functionality is done
                            progressFunctionality.PercentComplete = 100;
                            progressFunctionality.Detail = "Done";
                            progressFunctionality.Completed();

                            // Run Update Processes
                            UpdateProcesses(updateScripts, progressProcesses);

                            transaction.Complete();
                        }
                    }

                    // Clear out the pool so any connection holding onto SINGLE_USER gets released
                    SqlConnection.ClearAllPools();

                    FileGrow(toVersion);

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
        /// Run the update processes in the UpdateQueue
        /// </summary>
        private static void UpdateProcesses(List<SqlUpdateScript> updateScripts, ProgressItem progress)
        {
            IEnumerable<string> updateProcessNames = updateScripts
                .Where(script => !string.IsNullOrEmpty(script.UpdateProcessName))
                .Select(script => script.UpdateProcessName);

            progress.Starting();
            progress.Detail = "Processing updates from Update Queue...";

            int queueCount = updateProcessNames.Count();
            int currentItem = 0;

            foreach (string processName in updateProcessNames)
            {
                progress.Detail = string.Format("Processing {0} of {1}", currentItem, queueCount);
                progress.PercentComplete = Math.Min(100, currentItem/queueCount*100);
                
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    IUpdateDatabaseProcess updateDatabaseProcess = GetUpdateProcess(processName);
                    updateDatabaseProcess.Process();
                    transactionScope.Complete();
                }
            }

            progress.PercentComplete = 100;
            progress.Detail = "Done";
            progress.Completed();
        }

        /// <summary>
        /// Gets the update process.
        /// </summary>
        private static IUpdateDatabaseProcess GetUpdateProcess(string processName)
        {
            Type updateDatabaseProcessType = Type.GetType(processName);
            if (updateDatabaseProcessType == null)
            {
                throw new InvalidOperationException(string.Format("Unknown ProcessType {0}.", processName));                
            }

            return (IUpdateDatabaseProcess)Activator.CreateInstance(updateDatabaseProcessType, false);
        }

        /// <summary>
        /// If we were upgrading from 3.1.21 or before we adjust the FILEGROW settings.  Can't be in a transaction, so has to be here.
        /// </summary>
        private static void FileGrow(SchemaVersion installed)
        {
            if (installed.IsSystemVersion && installed.GetVersion() < new Version(3, 1, 21, 0))
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
        /// Updates the named schema version stored procedure.
        /// </summary>
        public static void UpdateSchemaVersionStoredProcedure(SqlConnection con, SchemaVersion version)
        {
            UpdateSchemaVersionStoredProcedure(con, new SchemaVersion("3.99.9.9"), "GetSchemaVersion");
            UpdateSchemaVersionStoredProcedure(con, version, "GetNamedSchemaVersion");
        }

        /// <summary>
        /// Update the schema version stored procedure to say the current schema is the given version
        /// </summary>
        private static void UpdateSchemaVersionStoredProcedure(SqlConnection con, SchemaVersion version, string storedProcName)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }

            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                cmd.CommandText = string.Format(@"
                IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    DROP PROCEDURE [dbo].[{0}]", storedProcName);
                
                SqlCommandProvider.ExecuteNonQuery(cmd);

#if DEBUG
                string withEncryption = "";
#else
                string withEncryption = "WITH ENCRYPTION";
#endif

                cmd.CommandText = string.Format(@"
                CREATE PROCEDURE dbo.{0} 
                {1}
                AS 
                SELECT '{2}' AS 'SchemaVersion'", storedProcName, withEncryption, version);
                SqlCommandProvider.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Upgrade a 3.x database to the current version.
        /// </summary>
        private static void UpdateScripts(List<SqlUpdateScript> updateScripts, ProgressItem progress)
        {
            progress.Starting();

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

                string reseedScript = GetReseedScript(con);

                // Go through each update script (they are already sorted in version order)
                foreach (SqlUpdateScript script in updateScripts)
                {
                    log.InfoFormat("Updating to {0}", script.SchemaVersion);

                    // How many scripts we've finished with so far
                    int scriptsCompleted = updateScripts.IndexOf(script);

                    // Execute the script
                    SqlScript executor = sqlLoader[script.ScriptName];
                    executor.AppendSql(sqlLoader.GetScript(GetDataScriptName(script.ScriptName), false));
                    executor.AppendSql(GetUpdateProcessSqlScript(script.UpdateProcessName));

                    // Update the progress as we complete each bactch in the script
                    executor.BatchCompleted += delegate(object sender, SqlScriptBatchCompletedEventArgs args)
                    {
                        int percentComplete = Math.Min(100, ((int)(scriptsCompleted*scriptProgressValue)) + (int)((args.Batch + 1)*(scriptProgressValue/executor.BatchCount)));

                        if (percentComplete < 0)
                        {
                            Debug.Write(percentComplete);    
                        }
                        
                        // Update the progress
                        progress.PercentComplete = percentComplete;
                    };

                    // Run all the batches in the script
                    executor.Execute(con);
                }

                progress.PercentComplete = 99;
                progress.Detail = "Reseeding Database";
                SqlScript reseedExecutor = new SqlScript("Reseed", reseedScript);
                reseedExecutor.Execute(con);

                progress.PercentComplete = 100;
                progress.Detail = "Done";
                progress.Completed();
            }
        }

        /// <summary>
        /// Gets the reseed script.
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        private static string GetReseedScript(SqlConnection con)
        {
            StringBuilder reseedScript = new StringBuilder();
            using (SqlCommand command = new SqlCommand(@"
                    SELECT IDENT_CURRENT(TABLE_NAME) AS Current_Identity,
                    TABLE_NAME
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE OBJECTPROPERTY(OBJECT_ID(TABLE_NAME), 'TableHasIdentity') = 1
                    AND TABLE_TYPE = 'BASE TABLE'", con))
            {
                using (SqlDataReader sqlDataReader = SqlCommandProvider.ExecuteReader(command))
                {
                    while (sqlDataReader.Read())
                    {
                        string tableName = sqlDataReader.GetString(1);
                        SqlDecimal seed = sqlDataReader.GetSqlDecimal(0);
                        reseedScript.AppendLine(string.Format("DBCC CHECKIDENT(N'[{0}]', RESEED, {1})", tableName, seed.ToString()));
                        reseedScript.AppendLine("GO");
                    }
                }
            }
            return reseedScript.ToString();
        }

        /// <summary>
        /// Gets the update process SQL script.
        /// </summary>
        private static string GetUpdateProcessSqlScript(string updateProcessName)
        {
            if (string.IsNullOrEmpty(updateProcessName))
            {
                return string.Empty;
            }

            return string.Format(
                "INSERT INTO UpdateQueue (UpdateDatabaseProcessType) VALUES ('{0}'){1}GO",
                updateProcessName,
                Environment.NewLine);
        }

        /// <summary>
        /// Gets the name of the data script.
        /// </summary>
        private static string GetDataScriptName(string scriptName)
        {
            scriptName = scriptName.Replace(".sql", string.Empty);
            return string.Format("{0}.data.sql", scriptName);
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
                string installingSchemaVersion = null;

                // Need to extract the type
                OptionSet optionSet = new OptionSet()
                {
                    { "dbschema=", s => installingSchemaVersion = s },
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

                        SchemaVersion databaseSchemaVersion = GetDatabaseSchemaVersion();

                        log.InfoFormat("Database schema version  {0}", databaseSchemaVersion);

                        SchemaVersionComparisonResult schemaVersionComparisonResult = databaseSchemaVersion.Compare(new SchemaVersion(installingSchemaVersion));

                        Environment.ExitCode = schemaVersionComparisonResult == SchemaVersionComparisonResult.Newer ? 1 : 0;
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