using ShipWorks.Users.Audit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Interapptive.Shared.Utility;
using log4net;
using System.Data.SqlClient;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Class for logging ConnectionMonitor exceptions
    /// </summary>
    public class ConnectionMonitorExceptionLogger
    {
        private ILog log;
        private StringBuilder msg;

        /// <summary>
        /// Log an exception, and any of it's inner exceptions
        /// </summary>
        public void Log(Exception exception, StringBuilder message, ILog log)
        {
            if (exception == null || log == null)
            {
                return;
            }

            if (message == null)
            {
                message = new StringBuilder();
            }

            this.log = log;
            this.msg = message;

            IEnumerable<Exception> exceptions = exception.GetAllExceptions();

            foreach (Exception ex in exceptions)
            {
                try
                {
                    LogException(ex);
                }
                catch (Exception subException)
                {
                    // Just log and continue if an error is thrown.
                    log.Error("An exception occurred while attempting to log exceptions.", subException);
                }
            }
        }

        /// <summary>
        /// Write exception specific info to the StringBuilder
        /// </summary>
        private void LogException(Exception ex)
        {
            msg.AppendLine("Exception message: ");
            msg.AppendLine(ex.Message);
            msg.AppendLine($"HResult: { ex.HResult }");

            LogComException(ex);

            LogSqlException(ex);

            LogTransactionInfo(ex);

            msg.AppendLine($"AuditBehaviorScope.ActiveState: { AuditBehaviorScope.ActiveState }");
            msg.AppendLine($"AuditBehaviorScope.ActiveReason: { AuditBehaviorScope.ActiveReason }");

            log.Error(msg.ToString(), ex);

            Exception baseEx = ex.GetBaseException();
            if (baseEx != null && baseEx != ex)
            {
                msg.AppendLine("Base Exception:");
                LogException(baseEx);
            }
        }

        /// <summary>
        /// Log SqlException specific info
        /// </summary>
        private void LogSqlException(Exception ex)
        {
            if (ex is SqlException)
            {
                SqlException sqlException = ex as SqlException;
                msg.AppendLine($"SqlException Number: { sqlException.Number }");
                msg.AppendLine($"SqlException Class: { sqlException.Class }");
                msg.AppendLine($"SqlException ClientConnectionId: { sqlException.ClientConnectionId }");
                msg.AppendLine($"SqlException ErrorCode: { sqlException.ErrorCode }");

                foreach (SqlError error in sqlException.Errors)
                {
                    msg.AppendLine($"SqlException Error:");
                    msg.AppendLine($"     Class: { error.Class }");
                    msg.AppendLine($"     LineNumber: { error.LineNumber }");
                    msg.AppendLine($"     Message: { error.Message }");
                    msg.AppendLine($"     Number: { error.Number }");
                    msg.AppendLine($"     Procedure: { error.Procedure }");
                    msg.AppendLine($"     State: { error.State }");
                }

                msg.AppendLine($"SqlException LineNumber: { sqlException.LineNumber }");
                msg.AppendLine($"SqlException Procedure: { sqlException.Procedure }");
                msg.AppendLine($"SqlException Server: { sqlException.Server }");
                msg.AppendLine($"SqlException State: { sqlException.State }");
            }
        }

        /// <summary>
        /// Log COMException specific info
        /// </summary>
        private void LogComException(Exception ex)
        {
            if (ex is COMException)
            {
                msg.AppendLine($"COMException ErrorCode: { ((COMException) ex).ErrorCode }");
            }
        }

        /// <summary>
        /// Log Transaction specific info
        /// </summary>
        private void LogTransactionInfo(Exception ex)
        {
            // This is probably an MSDTC or "remote/network transactions" exception.  
            // We're trying to find out why this is happening, so get some extra logging info
            TransactionInformation tranInfo = Transaction.Current?.TransactionInformation;
            
            if (tranInfo != null)
            {
                msg.AppendLine("TransactionInfo:");
                msg.AppendLine($"     DistributedIdentifier: { tranInfo.DistributedIdentifier }");
                msg.AppendLine($"     LocalIdentifier: { tranInfo.LocalIdentifier }");
                msg.AppendLine($"     Status: { tranInfo.Status }");
            }
        }
    }
}
