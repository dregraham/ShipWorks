using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Data;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.CommandLineOptions
{
    /// <summary>
    /// Command line option for upgrading the database schema
    /// </summary>
    public class UpgradeDatabaseSchemaCommandLineOption : ICommandLineCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UpgradeDatabaseSchemaCommandLineOption));
        private IAutoUpdateStatusProvider autoUpdateStatusProvider = new AutoUpdateStatusProvider();

        /// <summary>
        /// The CommandName that can be sent to the ShipWorks.exe
        /// </summary>
        public string CommandName => "upgradedatabaseschema";

        /// <summary>
        /// Execute the command
        /// </summary>
        public Task Execute(List<string> args)
        {
            Version versionRequired = new Version();
            TelemetricResult<Unit> databaseUpdateResult = new TelemetricResult<Unit>("Database.Update");
            TelemetricResult<Result> backupResult = null;
            AutoUpgradeFailureSubmitter autoUpgradeFailureSubmitter = new AutoUpgradeFailureSubmitter();

            try
            {
                // before doing anything make sure we can connect to the database and an upgrade is required
                SqlSession.Initialize();

                if (!SqlSession.Current.CanConnect())
                {
                    throw new Exception("Cannot connect to SQL Server. Not running upgrade.");
                }

                autoUpgradeFailureSubmitter.Initialize();

                versionRequired = SqlSchemaUpdater.GetRequiredSchemaVersion();

                if (SqlSchemaUpdater.IsUpgradeRequired())
                {
                    DatabaseUpgradeBackupManager backupManager = new DatabaseUpgradeBackupManager();
                    autoUpdateStatusProvider.UpdateStatus("Creating Backup");

                    if (SqlServerInfo.HasCustomTriggers())
                    {
                        throw new Exception("Database has custom triggers and cannot be automatically upgraded.");
                    }

                    autoUpdateStatusProvider.UpdateStatus("Creating Backup");
                    // If an upgrade is required create a backup first
                    backupResult = backupManager.CreateBackup(autoUpdateStatusProvider.UpdateStatus);

                    if (backupResult.Value.Success)
                    {
                        autoUpdateStatusProvider.UpdateStatus("Upgrading Database");
                        TryDatabaseUpgrade(backupManager, databaseUpdateResult);
                    }
                }

                // If we can't connect now, try to set back to multi-user
                if (!SqlSession.Current.CanConnect())
                {
                    SqlUtility.SetMultiUser(SqlSession.Current.Configuration.GetConnectionString(), SqlSession.Current.Configuration.DatabaseName);
                }
            }
            catch (Exception ex)
            {
                DatabaseUpgradeTelemetry.ExtractErrorDataForTelemetry(databaseUpdateResult, ex);
                log.Error("Failed to upgrade database schema", ex);

                if (ex is SqlException sqlEx)
                {
                    Environment.ExitCode = sqlEx.Number;
                }

                Environment.ExitCode = -1;

                autoUpgradeFailureSubmitter.Submit(versionRequired.ToString(), ex);
            }
            finally
            {
                SubmitTelemetryTelemetry(databaseUpdateResult, backupResult);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Try to upgrade the database, restore if it fails
        /// </summary>
        private TelemetricResult<Unit> TryDatabaseUpgrade(DatabaseUpgradeBackupManager backupManager, TelemetricResult<Unit> databaseUpdateResult)
        {
            try
            {
                databaseUpdateResult.RunTimedEvent(TelemetricEventType.SchemaUpdate,
                    () => SqlSchemaUpdater.UpdateDatabase(new ProgressProvider(), databaseUpdateResult));
            }
            catch (Exception)
            {
                autoUpdateStatusProvider.UpdateStatus("An error occurred during upgrade, rolling back.");
                // Upgrading the schema failed, restore
                databaseUpdateResult.RunTimedEvent("RestoreBackupTimeInMilliseconds", () => backupManager.RestoreBackup());
                throw;
            }

            return databaseUpdateResult;
        }

        /// <summary>
        /// Submits telemetry for the operation
        /// </summary>
        private void SubmitTelemetryTelemetry(TelemetricResult<Unit> databaseUpdateResult, TelemetricResult<Result> backupResult)
        {
            if (SqlSession.Current.CanConnect())
            {
                DatabaseUpgradeTelemetry.RecordDatabaseTelemetry(databaseUpdateResult);
            }

            using (ITrackedEvent telementryEvent = new TrackedEvent("Database.Update"))
            {
                telementryEvent.AddProperty("Mode", "CommandLine");
                telementryEvent.AddProperty("MachineName", Environment.MachineName);
                databaseUpdateResult.WriteTo(telementryEvent);
                backupResult?.WriteTo(telementryEvent);
            }

            // force all the telemetry data from above to flushed
            Telemetry.Flush();
            // Give it time to finish flushing
            Thread.Sleep(5000);
        }
    }
}
