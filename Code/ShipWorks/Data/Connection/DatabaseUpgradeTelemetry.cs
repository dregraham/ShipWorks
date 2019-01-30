using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Collects DatabaseUpgrade specific telemetry
    /// </summary>
    public static class DatabaseUpgradeTelemetry
    {
        /// <summary>
        /// Record database telemetry data to the telemetric result
        /// </summary>
        public static void RecordDatabaseTelemetry(TelemetricResult<Unit> telemetricResult)
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                SqlUtility.RecordDatabaseTelemetry(con, telemetricResult);
            }

            telemetricResult.AddProperty("IsWindowsAuth", SqlSession.Current.Configuration.WindowsAuth.ToString());
            telemetricResult.AddProperty("IsServerMachine", SqlSession.Current.IsLocalServer().ToString());
        }

        /// <summary>
        /// Extract data from the exception caused by an upgrade for telemetry
        /// </summary>
        public static void ExtractErrorDataForTelemetry(TelemetricResult<Unit> telemetricResult, Exception ex)
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
