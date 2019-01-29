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
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    SqlUtility.RecordDatabaseTelemetry(con, databaseUpdateResult);
                }

                databaseUpdateResult.AddProperty("IsWindowsAuth", SqlSession.Current.Configuration.WindowsAuth.ToString());
                databaseUpdateResult.AddProperty("IsServerMachine", SqlSession.Current.IsLocalServer().ToString());

                databaseUpdateResult.RunTimedEvent(TelemetricEventType.SchemaUpdate,
                    () => SqlSchemaUpdater.UpdateDatabase(new ProgressProvider(), databaseUpdateResult));
            }
            catch (SqlException ex)
            {
                ExtractErrorDataForTelemetry(databaseUpdateResult, ex);
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

        /// <summary>
        /// Extract data from the exception for telemetry
        /// </summary>
        private static void ExtractErrorDataForTelemetry(TelemetricResult<Unit> telemetricResult, Exception ex)
        {
            // if the exception is a SqlScriptException the message contains info about which
            // sql script caused the exception
            telemetricResult.AddProperty("ExceptionMessage", ex.Message);
            telemetricResult.AddProperty("ExceptionType", ex.GetType().ToString());

            if (ex is SqlException sqlException)
            {
                telemetricResult.AddProperty("ErrorCode", sqlException.ErrorCode.ToString());
            }
        }
    }
}
