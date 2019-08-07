using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
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
            AddException(telemetricResult, ex, string.Empty, string.Empty);

            int innerExCount = 0;

            ex.InnerException?.GetAllExceptions().ForEach(exception => {
                innerExCount++;
                AddException(telemetricResult, ex, "Inner", innerExCount.ToString());
            });
        }

        /// <summary>
        /// Add exception details to telemetry
        /// </summary>
        private static void AddException(TelemetricResult<Unit> telemetricResult, Exception ex, string prefix, string postfix)
        {
            // if the exception is a SqlScriptException the message contains info about which
            // sql script caused the exception
            telemetricResult.AddProperty($"{prefix}ExceptionMessage{postfix}", ex.Message);
            telemetricResult.AddProperty($"{prefix}ExceptionStackTrace{postfix}", ex.StackTrace);
            telemetricResult.AddProperty($"{prefix}ExceptionType{postfix}", ex.GetType().ToString());

            if (ex is SqlException sqlEx)
            {
                telemetricResult.AddProperty($"{prefix}ErrorCode{postfix}", sqlEx.ErrorCode.ToString());
            }
        }
    }
}
