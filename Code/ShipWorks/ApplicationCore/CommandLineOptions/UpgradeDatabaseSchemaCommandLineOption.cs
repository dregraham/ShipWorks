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
            SchemaUpgradeBackupManager backupManager = new SchemaUpgradeBackupManager();

            TelemetricResult<Unit> databaseUpdateResult = new TelemetricResult<Unit>("Database.Update");
            databaseUpdateResult.AddProperty("Mode", "CommandLine");

            TelemetricResult<string> backupResult = null;

            try
            {
                SqlSession.Initialize();

                DatabaseUpgradeTelemetry.RecordDatabaseTelemetry(databaseUpdateResult);

                // If an upgrade is required create a backup first
                if (SqlSchemaUpdater.IsUpgradeRequired())
                {
                    backupResult = backupManager.CreateBackup();

                    databaseUpdateResult.RunTimedEvent(TelemetricEventType.SchemaUpdate,
                        () => SqlSchemaUpdater.UpdateDatabase(new ProgressProvider(), databaseUpdateResult));
                    

                    // If it fails call 
                    // backupManager.RestoreBackup();
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
            }
            finally
            {
                using (ITrackedEvent telementryEvent = new TrackedEvent("Database.Update"))
                {
                    databaseUpdateResult.WriteTo(telementryEvent);
                    backupResult?.WriteTo(telementryEvent);
                }

				// force all the telemetry data from above to flushed
				Telemetry.Flush();
				// Give it time to finish flushing
				Thread.Sleep(5000);
			}

            return Task.CompletedTask;
        }
    }
}
