using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reactive;
using System.Threading.Tasks;
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
            TelemetricResult<Unit> databaseUpdateResult = new TelemetricResult<Unit>("Database.Update");
            databaseUpdateResult.AddProperty("Mode", "CommandLine");

            try
            {
                SqlSession.Initialize();

                DatabaseUpgradeTelemetry.RecordDatabaseTelemetry(databaseUpdateResult);

                databaseUpdateResult.RunTimedEvent(TelemetricEventType.SchemaUpdate,
                    () => SqlSchemaUpdater.UpdateDatabase(new ProgressProvider(), databaseUpdateResult));
            }
            catch (SqlException ex)
            {
                DatabaseUpgradeTelemetry.ExtractErrorDataForTelemetry(databaseUpdateResult, ex);
                log.Error("Failed to upgrade database schema", ex);
                Environment.ExitCode = ex.ErrorCode;
            }
            finally
            {
                using (ITrackedEvent telementryEvent = new TrackedEvent("Database.Update"))
                {
                    databaseUpdateResult.WriteTo(telementryEvent);
                }
            }

            return Task.CompletedTask;
        }
    }
}
