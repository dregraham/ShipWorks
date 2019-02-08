using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
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
        static readonly ILog log = LogManager.GetLogger(typeof(UpgradeDatabaseSchemaCommandLineOption));

        /// <summary>
        /// The CommandName that can be sent to the ShipWorks.exe
        /// </summary>
        public string CommandName => "upgradedatabaseschema";

        /// <summary>
        /// Execute the command
        /// </summary>
        public Task Execute(List<string> args)
        {
            // before doing anything make sure we can connect to the database and an upgrade is required
            SqlSession.Initialize();
            if (SqlSchemaUpdater.IsUpgradeRequired())
            {
                TelemetricResult<Unit> databaseUpdateResult = new TelemetricResult<Unit>("Database.Update");
                TelemetricResult<string> backupResult = null;

                SchemaUpgradeBackupManager backupManager = new SchemaUpgradeBackupManager();
                try
                {
                    // If an upgrade is required create a backup first
                    backupResult = backupManager.CreateBackup();
                    TryDatabaseUpgrade(backupManager, databaseUpdateResult);

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
                }
                finally
                {
                    SubmitTelemetryTelemetry(databaseUpdateResult, backupResult);
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Try to upgrade the database, restore if it fails
        /// </summary>
        private TelemetricResult<Unit> TryDatabaseUpgrade(SchemaUpgradeBackupManager backupManager, TelemetricResult<Unit> databaseUpdateResult)
        {
            
            try
            {
                databaseUpdateResult.RunTimedEvent(TelemetricEventType.SchemaUpdate,
                    () => SqlSchemaUpdater.UpdateDatabase(new ProgressProvider(), databaseUpdateResult));
            }
            catch (Exception)
            {
                // Upgrading the schema failed, restore
                databaseUpdateResult.RunTimedEvent("RestoreBackupTimeInMilliseconds", () => backupManager.RestoreBackup());
                throw;
            }

            return databaseUpdateResult;
        }

        /// <summary>
        /// Submits telemetry for the operation
        /// </summary>
        private void SubmitTelemetryTelemetry(TelemetricResult<Unit> databaseUpdateResult, TelemetricResult<string> backupResult)
        {
            DatabaseUpgradeTelemetry.RecordDatabaseTelemetry(databaseUpdateResult);

            using (ITrackedEvent telementryEvent = new TrackedEvent("Database.Update"))
            {
                telementryEvent.AddProperty("Mode", "CommandLine");
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
