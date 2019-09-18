using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Data;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Licensing;
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
            log.Info("Execute starting.");

            string tangoCustomerId = string.Empty;
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

                // Cache the result from TangoWebClient.GetTangoCustomerId() now so that if the db connection
                // is lost, we can still track the customer id.
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    ITangoWebClient tangoWebClient = scope.Resolve<ITangoWebClient>();
                    tangoCustomerId = tangoWebClient.GetTangoCustomerId();
                }

                Telemetry.GetCustomerID = () => tangoCustomerId;
                log.Info($"TangoCustomerId: {tangoCustomerId}.");

                autoUpgradeFailureSubmitter.Initialize();

                versionRequired = SqlSchemaUpdater.GetRequiredSchemaVersion();
                log.Info($"RequiredSchemaVersion: {versionRequired}.");

                if (SqlSchemaUpdater.IsUpgradeRequired())
                {
                    log.Info("IsUpgradeRequired: true.");
                    DatabaseUpgradeBackupManager backupManager = new DatabaseUpgradeBackupManager();
                    backupResult = BackupDatabase(backupManager);

                    if (backupResult.Value.Success)
                    {
                        autoUpdateStatusProvider.UpdateStatus("Upgrading Database");
                        TryDatabaseUpgrade(backupManager, databaseUpdateResult);
                    }
                    else
                    {
                        log.Error($"Backup failed.  {backupResult.Value.Message}");
                    }
                }

                SetMultiUserIfNeeded();
            }
            catch (Exception ex)
            {
                HandleException(databaseUpdateResult, ex, autoUpgradeFailureSubmitter, versionRequired);
            }
            finally
            {
                SubmitTelemetryTelemetry(databaseUpdateResult, backupResult);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Set the db to multi user if needed
        /// </summary>
        private static void SetMultiUserIfNeeded()
        {
            // If we can't connect now, try to set back to multi-user
            if (!SqlSession.Current.CanConnect())
            {
                log.Info("Unable to connect to db, so trying to set to multi user.");
                SqlUtility.SetMultiUser(SqlSession.Current.Configuration.GetConnectionString(), SqlSession.Current.Configuration.DatabaseName);
            }
        }

        /// <summary>
        /// Backup the database
        /// </summary>
        private TelemetricResult<Result> BackupDatabase(DatabaseUpgradeBackupManager backupManager)
        {
            autoUpdateStatusProvider.UpdateStatus("Creating Backup");

            if (SqlServerInfo.HasCustomTriggers())
            {
                throw new Exception("Database has custom triggers and cannot be automatically upgraded.");
            }

            autoUpdateStatusProvider.UpdateStatus("Creating Backup");

            // If an upgrade is required create a backup first
            return backupManager.CreateBackup(autoUpdateStatusProvider.UpdateStatus);
        }

        /// <summary>
        /// Handle an exception
        /// </summary>
        private static void HandleException(TelemetricResult<Unit> databaseUpdateResult, Exception ex, AutoUpgradeFailureSubmitter autoUpgradeFailureSubmitter, Version versionRequired)
        {
            DatabaseUpgradeTelemetry.ExtractErrorDataForTelemetry(databaseUpdateResult, ex);
            log.Error("Failed to upgrade database.", ex);

            if (ex is SqlException sqlEx)
            {
                Environment.ExitCode = sqlEx.Number;
            }

            Environment.ExitCode = -1;

            autoUpgradeFailureSubmitter.Submit(versionRequired.ToString(), ex);
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
            catch (Exception ex)
            {
                log.Error("TryDatabaseUpgrade had an exception.", ex);
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
