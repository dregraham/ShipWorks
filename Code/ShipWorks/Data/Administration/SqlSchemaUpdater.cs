using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using log4net;
using System.Reflection;
using Interapptive.Shared;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;
using ShipWorks.Common.Threading;
using ShipWorks.Filters;
using ShipWorks.Users.Audit;
using ShipWorks.ApplicationCore.Interaction;
using NDesk.Options;
using ShipWorks.Actions;
using ShipWorks.Actions.Scheduling.ActionSchedules.Enums;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Administration.UpdateFrom2x.Database;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore;
using Autofac;
using ShipWorks.ApplicationCore.Licensing;

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
        static SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Data.Administration.Scripts.Update");

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
        private static Version GetInstalledAssemblyVersion()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                try
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con, "GetAssemblySchemaVersion"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        return new Version((string) SqlCommandProvider.ExecuteScalar(cmd));
                    }
                }
                catch (SqlException ex)
                {
                    // "Could not find stored procedure"
                    if (ex.Number == 2812 || ex.Number == 21343)
                    {
                        return new Version(3,0);
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
        /// Upgrade the current database to the latest version.  debuggingMode is only provided as an option for debugging purposes, and should always be false in 
        /// customer or production scenarios.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void UpdateDatabase(ProgressProvider progressProvider, bool debuggingMode = false)
        {
            Version installed = GetInstalledSchemaVersion();

            log.InfoFormat("Upgrading database from {0} to {1}", installed, GetRequiredSchemaVersion());

            // Create the primary progress item
            ProgressItem progressScripts = new ProgressItem("Update Tables");
            progressScripts.CanCancel = false;
            progressProvider.ProgressItems.Add(progressScripts);

            // Create the functionality item
            ProgressItem progressFunctionality = new ProgressItem("Update Functionality");
            progressFunctionality.CanCancel = false;
            progressProvider.ProgressItems.Add(progressFunctionality);
           
            // Start by disconnecting all users. Allow for a long timeout while trying to regain a connection when in single user mode
            // because reconnection to a very large database seems to take some time after running a big upgrade
            using (SingleUserModeScope singleUserScope = debuggingMode ? null : new SingleUserModeScope(TimeSpan.FromMinutes(1)))
            {
                try
                {
                    // Put the SuperUser in scope, and don't audit
                    using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
                    {
                        using (new ExistingConnectionScope())
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
                                UpdateFilters(progressFunctionality, ExistingConnectionScope.ScopedConnection, ExistingConnectionScope.ScopedTransaction);
                            }

                            // Functionality is done
                            progressFunctionality.PercentComplete = 100;
                            progressFunctionality.Detail = "Done";
                            progressFunctionality.Completed();

                            // If we were upgrading from 3.9.3.0 or before we adjust the FILEGROW settings.  Can't be in a transaction, so has to be here.
                            if (installed <= new Version(3, 9, 3, 0))
                            {
                                ExistingConnectionScope.ExecuteWithCommand(cmd =>
                                {
                                    cmd.CommandText = @"

                                        DECLARE @dbName nvarchar(100)
                                        DECLARE @isAutoShrink int

                                        SET @dbName = DB_NAME()

                                        SELECT @isAutoShrink = CONVERT(int,DATABASEPROPERTYEX([Name] , 'IsAutoShrink'))
                                        FROM master.dbo.sysdatabases
                                        where name = @dbName

                                        IF(@isAutoShrink = 1)
	                                        EXECUTE ('ALTER DATABASE ' + @dbName + ' SET AUTO_SHRINK OFF')";

                                    cmd.ExecuteNonQuery();
                                });

                                // Update size and growth of shipworks database
                                ExistingConnectionScope.ExecuteWithCommand(cmd =>
                                {
                                    cmd.CommandText = @"
                                        DECLARE @logSize int 
                                        DECLARE @dataSize int 
                                        DECLARE @dataFileGrowth int 
                                        DECLARE @logFileGrowth int
                                        DECLARE @dataName nvarchar(100) 
                                        DECLARE @logName nvarchar(100)
                                        DECLARE @dbName nvarchar(100)

                                        SET @dbName = DB_NAME()

                                        SELECT @dataSize = SUM(CASE WHEN type_desc = 'ROWS' THEN size END),
                                               @dataName = MAX(CASE WHEN type_desc = 'ROWS' THEN name END),
                                               @dataFileGrowth = SUM(CASE WHEN type_desc = 'ROWS' AND is_percent_growth=1 THEN growth ELSE 0 END),
                                               @logSize = SUM(CASE WHEN type_desc = 'LOG' THEN size END),
                                               @logName = MAX(CASE WHEN type_desc = 'LOG' THEN name END),
                                               @logFileGrowth = SUM(CASE WHEN type_desc = 'LOG' AND is_percent_growth=1 THEN growth ELSE 0 END)
                                        FROM sys.master_files 
                                        where DB_NAME(database_id) = @dbName

                                        IF (@logSize < 25600)
                                            EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @logName + ''', SIZE = 200MB)' )

                                        IF (@dataSize < 25600)
                                            EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @dataName + ''', SIZE = 200MB)' )

                                        IF (@dataFileGrowth < 25600 OR @dataFileGrowth >= 64000)
                                            EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @dataName + ''', FILEGROWTH = 200MB)' )

                                        IF (@logFileGrowth < 25600 OR @logFileGrowth >= 64000)
                                            EXECUTE ('ALTER DATABASE ' + @dbName + ' MODIFY FILE ( NAME = N''' + @logName + ''', FILEGROWTH = 200MB)' )
                                    ";

                                    cmd.ExecuteNonQuery();
                                });

                                // Update size and growth of tempdb
                                ExistingConnectionScope.ExecuteWithCommand(cmd =>
                                {
                                    cmd.CommandText = @"
                                        DECLARE @logSize int 
                                        DECLARE @dataSize int 
                                        DECLARE @dataFileGrowth int 
                                        DECLARE @logFileGrowth int
                                        DECLARE @dataName nvarchar(100) 
                                        DECLARE @logName nvarchar(100)

                                        SELECT @dataSize = SUM(CASE WHEN type_desc = 'ROWS' THEN size END),
	                                           @dataName = MAX(CASE WHEN type_desc = 'ROWS' THEN name END),
	                                           @dataFileGrowth = SUM(CASE WHEN type_desc = 'ROWS' AND is_percent_growth=1 THEN growth ELSE 0 END),
	                                           @logSize = SUM(CASE WHEN type_desc = 'LOG' THEN size END),
	                                           @logName = MAX(CASE WHEN type_desc = 'LOG' THEN name END),
	                                           @logFileGrowth = SUM(CASE WHEN type_desc = 'LOG' AND is_percent_growth=1 THEN growth ELSE 0 END)
                                        FROM sys.master_files 
                                        where DB_NAME(database_id) = 'tempdb'
	 
                                        IF (@logSize < 25600)
                                            EXECUTE ('ALTER DATABASE tempdb MODIFY FILE ( NAME = N''' + @logName + ''', SIZE = 200MB)' )

                                        IF (@dataSize < 25600)
                                            EXECUTE ('ALTER DATABASE tempdb MODIFY FILE ( NAME = N''' + @dataName + ''', SIZE = 200MB)' )

                                        IF (@dataFileGrowth < 25600)
                                            EXECUTE ('ALTER DATABASE tempdb MODIFY FILE ( NAME = N''' + @dataName + ''', FILEGROWTH = 200MB)' )

                                        IF (@logFileGrowth < 25600)
                                            EXECUTE ('ALTER DATABASE tempdb MODIFY FILE ( NAME = N''' + @logName + ''', FILEGROWTH = 200MB)' )
                                    ";

                                    cmd.ExecuteNonQuery();
                                });
                            }
                            
                            // If we were upgrading from this version, add AddressValidation filters.
                            if (installed < new Version(3, 12, 0, 0))
                            {
                                AddressValidationDatabaseUpgrade addressValidationDatabaseUpgrade = new AddressValidationDatabaseUpgrade();
                                ExistingConnectionScope.ExecuteWithAdapter(addressValidationDatabaseUpgrade.Upgrade);
                            }
                            
                            // This was needed for databases created before Beta6.  Any ALTER DATABASE statements must happen outside of transaction, so we had to put this here (and do it every time, even if not needed)
                            SqlUtility.SetChangeTrackingRetention(ExistingConnectionScope.ScopedConnection, 1);

                            // Try to restore multi-user mode with the existing connection, since re-acquiring a connection after a large
                            // database upgrade can take time and cause a timeout.
                            if (singleUserScope != null)
                            {
                                SingleUserModeScope.RestoreMultiUserMode(ExistingConnectionScope.ScopedConnection);
                            }

                            // If we were upgrading from this version, Regenerate scheduled actions 
                            // To fix issue caused by breaking out assemblies
                            if (installed < new Version(4, 5, 0, 0))
                            {
                                // Grab all of the actions that are enabled and schedule based
                                ActionManager.InitializeForCurrentSession();
                                IEnumerable<ActionEntity> actions = ActionManager.Actions.Where(a => a.Enabled && a.TriggerType == (int)ActionTriggerType.Scheduled);
                                using (SqlAdapter adapter = new SqlAdapter())
                                {
                                    foreach (ActionEntity action in actions)
                                    {
                                        // Some trigger's state depend on the enabled state of the action
                                        ScheduledTrigger scheduledTrigger = ActionManager.LoadTrigger(action) as ScheduledTrigger;

                                        if (scheduledTrigger?.Schedule != null )
                                        {
                                            // Check to see if the action is a One Time action and in the past, if so we disable it
                                            if (scheduledTrigger.Schedule.StartDateTimeInUtc < DateTime.UtcNow &&
                                                scheduledTrigger.Schedule.ScheduleType == ActionScheduleType.OneTime)
                                            {
                                                action.Enabled = false;
                                            }
                                            else
                                            {
                                                scheduledTrigger.SaveExtraState(action, adapter);
                                            }
                                        }
                                        
                                        ActionManager.SaveAction(action, adapter);
                                    }

                                    adapter.Commit();
                                }
                            }

                            // update Configuration table Key column to have an encrypted empty string 
                            // using the GetDatabaseGuid stored procedure as the salt. 
                            if (installed < new Version(4, 9, 0, 0))
                            {
                                // Resolve a CustomerLicense passing in an empty string as the key parameter
                                CustomerLicense customerLicense = IoC.UnsafeGlobalLifetimeScope.Resolve<CustomerLicense>(new TypedParameter(typeof(string), string.Empty));

                                // Resolve the CustomerLicenseWriter needed to save the customer license to the database
                                ICustomerLicenseWriter writer = IoC.UnsafeGlobalLifetimeScope.Resolve<ICustomerLicenseWriter>();

                                // Write the license
                                writer.Write(customerLicense);
                            }

                        }
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
        /// Get a list of all the update scripts in ShipWorks, ordered from smallest version to greatest.
        /// </summary>
        public static List<SqlUpdateScript> GetUpdateScripts()
        {
            List<SqlUpdateScript> scripts = new List<SqlUpdateScript>();

            foreach (string resource in Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(r => r.StartsWith(sqlLoader.ResourcePath)))
            {
                scripts.Add(new SqlUpdateScript(resource));
            }

            return scripts.OrderBy(s => s.SchemaVersion).ToList();
        }

        /// <summary>
        /// Update the schema version store procedure to match the required schema version.  This should only be called after installing or updating to the latest schema.
        /// </summary>
        public static void UpdateSchemaVersionStoredProcedure(SqlConnection con)
        {
            UpdateSchemaVersionStoredProcedure(con, GetRequiredSchemaVersion());
        }

        /// <summary>
        /// Update the schema version stored procedure to say the current schema is the given version
        /// </summary>
        public static void UpdateSchemaVersionStoredProcedure(SqlConnection con, Version version)
        {
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                UpdateSchemaVersionStoredProcedure(cmd, version);   
            }
        }

        /// <summary>
        /// Update the schema version stored procedure to say the current schema is the given version
        /// </summary>
        public static void UpdateSchemaVersionStoredProcedure(SqlCommand cmd, Version version)
        {
            UpdateVersionStoredProcedure(cmd, version, "GetSchemaVersion");
        }

        /// <summary>
        /// Update the assembly version stored procedure to say the current schema is the given version
        /// </summary>
        public static void UpdateAssemblyVersionStoredProcedure(SqlCommand cmd)
        {
            UpdateVersionStoredProcedure(cmd, GetRequiredSchemaVersion(), "GetAssemblySchemaVersion");
        }

        /// <summary>
        /// Update a stored procedure for checking a version
        /// </summary>
        private static void UpdateVersionStoredProcedure(SqlCommand cmd, Version version, string procedureName)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }

            cmd.CommandText = string.Format(@"
                IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                    DROP PROCEDURE [dbo].[{0}]", procedureName);
            SqlCommandProvider.ExecuteNonQuery(cmd);

            #if DEBUG
                string withEncryption = "";
            #else
                string withEncryption = "WITH ENCRYPTION";
            #endif

            cmd.CommandText = string.Format(@"
                CREATE PROCEDURE dbo.{2} 
                {0}
                AS 
                SELECT '{1}' AS 'SchemaVersion'", withEncryption, version.ToString(4), procedureName);
            SqlCommandProvider.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Upgrade a 3.x database to the current version.
        /// </summary>
        private static void UpdateScripts(Version installed, ProgressItem progress)
        {
            progress.Starting();
            progress.Detail = "Preparing...";

            // Get all the update scripts
            List<SqlUpdateScript> updateScripts = GetUpdateScripts().Where(s => s.SchemaVersion > installed).ToList();

            // Start with generic progress msg
            progress.Detail = "Updating...";

            // Store the event handler in a variable so it can be removed easily later
            SqlInfoMessageEventHandler infoMessageHandler = (sender, e) =>
            {
                if (progress.Status == ProgressItemStatus.Running)
                {
                    if (!e.Message.StartsWith("Caution"))
                    {
                        progress.Detail = e.Message;
                    }
                }
            };

            // Listen for script messages to display to the user
            ExistingConnectionScope.ScopedConnection.InfoMessage += infoMessageHandler;

            // Determine the percent-value of each update script
            double scriptProgressValue = 100.0 / (double) updateScripts.Count;

            // Go through each update script (they are already sorted in version order)
            foreach (SqlUpdateScript script in updateScripts)
            {
                ExistingConnectionScope.BeginTransaction();

                log.InfoFormat("Updating to {0}", script.SchemaVersion);

                // How many scripts we've finished with so far
                int scriptsCompleted = updateScripts.IndexOf(script);

                // Execute the script
                SqlScript executor = sqlLoader[script.ScriptName];

                // Update the progress as we complete each batch in the script
                executor.BatchCompleted += delegate(object sender, SqlScriptBatchCompletedEventArgs args)
                {
                    // Update the progress
                    progress.PercentComplete = Math.Min(100, ((int) (scriptsCompleted * scriptProgressValue)) + (int) ((args.Batch + 1) * (scriptProgressValue / executor.Batches.Count)));
                };

                // Run all the batches in the script
                ExistingConnectionScope.ExecuteWithCommand(executor.Execute);

                ExistingConnectionScope.ExecuteWithCommand(x => UpdateSchemaVersionStoredProcedure(x, script.SchemaVersion));
                
                ExistingConnectionScope.Commit();
            }

            // Since we have a single, long-lived connection, we want to remove the message handler so future messages
            // get handled normally
            ExistingConnectionScope.ScopedConnection.InfoMessage -= infoMessageHandler;

            progress.PercentComplete = 100;
            progress.Detail = "Done";
            progress.Completed();
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
        private static void UpdateFilters(ProgressItem progress, SqlConnection connection, SqlTransaction transaction)
        {
            progress.Detail = "Updating filters...";

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
            }
            finally
            {
                FilterLayoutContext.PopScope();
            }
        }

        /// <summary>
        /// Get the schema version of the ShipWorks database on the given connection
        /// </summary>
        public static Version GetInstalledSchemaVersion(SqlConnection con)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "GetSchemaVersion";
            cmd.CommandType = CommandType.StoredProcedure;

            return new Version((string) SqlCommandProvider.ExecuteScalar(cmd));
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
            public void Execute(List<string> args)
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

                        log.InfoFormat("Required shcema version: {0}", schemaID);
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
    }
}
