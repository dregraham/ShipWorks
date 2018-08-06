using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Globalization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using ShipWorks.Users.Audit;

namespace ShipWorks.SqlServer.General
{
    /// <summary>
    /// Helpful common utility functions to be used. 
    /// 
    /// Using the CompilerGenerated hack to enable use of static (not readonly) variables without
    /// specifying an UNSAFE assembly
    /// </summary>
    [CompilerGenerated]
    public static class UtilityFunctions
    {
        const string errorMissingHostInfo = "Values cannot be updated outside of ShipWorks.";

        // SQL Server resets the TransactionID every time it restarts.  When we initiall load, we capture what the max was when we loaded,
        // and add that to all our transactionID's.  That way, our transaction id's are always increasing.
        static long lastMaxTransactionID = 0;

        /// <summary>
        /// Get the mapping of column names to their ordinal positions in the table for the given table name and connection
        /// </summary>
        public static Dictionary<string, int> GetColumnNameOrdinalMap(SqlConnection con, string table)
        {
            SqlCommand columnsCmd = con.CreateCommand();
            columnsCmd.CommandText = string.Format("SELECT TOP (0) * FROM [{0}]", table);

            // We need to new which column names go with which ordinals
            Dictionary<string, int> ordinalMap = new Dictionary<string, int>();

            using (SqlDataReader reader = columnsCmd.ExecuteReader())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    ordinalMap[reader.GetName(i)] = i;
                }
            }

            return ordinalMap;
        }

        /// <summary>
        /// Get the name of the table affected by the current trigger
        /// </summary>
        public static string GetTriggerTableName()
        {
            TriggerAction triggerAction = SqlContext.TriggerContext.TriggerAction;

            return (triggerAction == TriggerAction.Delete) ? "deleted" : "inserted";
        }

        /// <summary>
        /// Get the current value of @@rowcount
        /// </summary>
        public static int GetLastRowCount(SqlConnection con)
        {
            SqlCommand cmdRowCount = con.CreateCommand();
            cmdRowCount.CommandText = "SELECT @@rowcount";

            return (int) cmdRowCount.ExecuteScalar();
        }

        /// <summary>
        /// Get all the primary keys from the table affected by the trigger
        /// </summary>
        public static List<long> GetTriggerPrimaryKeys(SqlConnection con)
        {
            return GetTriggerPrimaryKeys(con, "$IDENTITY");
        }

        /// <summary>
        /// Get all the primary keys from the table affected by the trigger
        /// </summary>
        public static List<long> GetTriggerPrimaryKeys(SqlConnection con, string primaryKey)
        {
            List<long> keys = new List<long>();

            // Assume a single PK column that is the first column
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = string.Format("SELECT {0} FROM {1}", primaryKey, GetTriggerTableName());

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    keys.Add((long) reader[0]);
                }
            }

            return keys;
        }

        /// <summary>
        /// Get user and computer information for who is currently running
        /// </summary>
        public static UserContext GetUserContext(SqlConnection con)
        {
            SqlCommand contextCmd = con.CreateCommand();
            contextCmd.CommandText = "SELECT CONVERT(VARCHAR(128), CONTEXT_INFO())";
            string contextInfo = contextCmd.ExecuteScalar() as string;

            // If its null, we may be trying to do an update from osql or Mgmnt Studio, so we'll try the old way.
            if (contextInfo == null)
            {
                SqlCommand hostCmd = con.CreateCommand();
                hostCmd.CommandText = "SELECT HOST_NAME()";
                contextInfo = ((string) hostCmd.ExecuteScalar()).Trim();

                if (contextInfo == null || contextInfo.Length < 10 || contextInfo.Substring(0, 10) == "0000000000")
                {
                    throw new InvalidOperationException(errorMissingHostInfo + $" contextInfo was null.");
                }
            }

            if (contextInfo.Substring(0, 10) == "0000000000")
            {
                throw new InvalidOperationException(errorMissingHostInfo + $" contextInfo(0, 10) was 0000000000 {contextInfo}.");
            }

            long computerID;
            if (!long.TryParse(contextInfo.Substring(5, 5), NumberStyles.AllowHexSpecifier, null, out computerID))
            {
                throw new InvalidOperationException(errorMissingHostInfo + $" Couldn't get computerID: {contextInfo.Substring(5, 5)}.");
            }

            computerID = (computerID * 1000) + 1;

            long userID;
            if (!long.TryParse(contextInfo.Substring(0, 5), NumberStyles.AllowHexSpecifier, null, out userID))
            {
                throw new InvalidOperationException(errorMissingHostInfo + $" Couldn't get userID: {contextInfo.Substring(0, 5)}.");
            }

            userID = (userID * 1000) + 2;

            int auditReasonType = 0;
            string auditReasonDetail = null;
            bool deletingStore = false;

            AuditState auditState = AuditState.Enabled;

            // See if the auditing byte is set
            if (contextInfo.Length >= 11 && (contextInfo[10] == 'E' || contextInfo[10] == 'D' || contextInfo[10] == 'S' || contextInfo[10] == 'P'))
            {
                // See if a store is being deleted
                deletingStore = contextInfo[10] == 'S';

                // Auditing is deleted for store deletions as well as when it's marked to be disabled
                if (deletingStore || contextInfo[10] == 'D')
                {
                    auditState = AuditState.Disabled;
                }
                else if (contextInfo[10] == 'P')
                {
                    auditState = AuditState.NoDetails;
                }

                // If auditing enabled is set, then audit reason may be set...
                if (contextInfo.Length >= 12)
                {
                    // The first byte is the type
                    if (!int.TryParse(contextInfo[11].ToString(), NumberStyles.AllowHexSpecifier, null, out auditReasonType))
                    {
                        throw new InvalidOperationException(errorMissingHostInfo);
                    }

                    int terminator = contextInfo.IndexOf("@;", 12);
                    if (terminator != -1)
                    {
                        if (terminator - 12 > 0)
                        {
                            auditReasonDetail = contextInfo.Substring(12, terminator - 12);
                        }
                    }
                }
            }

            return new UserContext(userID, computerID, auditReasonType, auditReasonDetail, auditState, deletingStore);
        }

        /// <summary>
        /// Get the ComputerID for the connection
        /// </summary>
        public static long GetComputerID(SqlConnection con)
        {
            return GetUserContext(con).ComputerID;
        }

        /// <summary>
        /// Get the UserID for the connection
        /// </summary>
        public static long GetUserID(SqlConnection con)
        {
            return GetUserContext(con).UserID;
        }

        /// <summary>
        /// Returns the current isolation level for the connection
        /// </summary>
        public static string GetTransactionLevel(SqlConnection con)
        {
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = @"
                select case transaction_isolation_level
                            when 1 then 'Read Uncomitted'
                            when 2 then 'Read Committed'
                            when 3 then 'Repeatable Read'
                            when 4 then 'Serializable'
                            when 5 then 'Snapshot'
                            else 'Unspecified'
                        end
                from   sys.dm_exec_sessions --or sys.dm_exec_requests, depending on what you want
                where  session_id = @@spid";

            return (string) cmd.ExecuteScalar();
        }

        /// <summary>
        /// Force the current @@DBTS value to increase so the ShipWorks heartbeat will pick up a change.  This is useful
        /// when deletes are done that otherwise would not cause a change.
        /// </summary>
        public static void IncreaseDbts(SqlConnection con)
        {
            SqlCommand cmd = con.CreateCommand();

            cmd.CommandText = @"
                SET DEADLOCK_PRIORITY -5
                update Dirty set Count = 1";

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get the current SQL server transaction ID.
        /// Internal b\c all callers should go through the SQL UserDefinedFunction instead
        /// </summary>
        internal static long GetTransactionID(SqlConnection con)
        {
            if (Interlocked.Read(ref lastMaxTransactionID) == 0)
            {
                SqlCommand maxCmd = con.CreateCommand();
                maxCmd.CommandText = "SELECT COALESCE(MAX(TransactionID), 0) FROM Audit";

                Interlocked.CompareExchange(ref lastMaxTransactionID, (long) maxCmd.ExecuteScalar(), 0);
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT transaction_id FROM sys.dm_tran_current_transaction";

            return ((long) cmd.ExecuteScalar()) + lastMaxTransactionID;
        }

        /// <summary>
        /// Throws an InvalidOperationException if there is a transaction open on the given connection
        /// </summary>
        public static void EnsureNotTransacted(SqlConnection con)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT @@TRANCOUNT";

            if ((int) cmd.ExecuteScalar() > 0)
            {
                throw new InvalidOperationException("An active transaction should not be open on this connection.");
            }
        }

        /// <summary>
        /// Indicates if the given exception represents a deadlock
        /// </summary>
        public static bool IsDeadlockException(SqlException ex)
        {
            return IsSpecificSqlException(ex, 1205, "deadlock victim");
        }

        /// <summary>
        /// Indicates if the given exception represents a constraint exception
        /// </summary>
        public static bool IsConstraintException(SqlException ex)
        {
            return IsSpecificSqlException(ex, 547, "statement conflicted with");
        }

        /// <summary>
        /// Set the active deadlock priority on the connection
        /// </summary>
        public static void SetDeadlockPriority(SqlConnection con, int priority)
        {
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = string.Format("SET DEADLOCK_PRIORITY {0}", priority);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks whether the SqlException is what we expect, either by the error number or by an error message
        /// </summary>
        private static bool IsSpecificSqlException(SqlException ex, int errorNumber, string errorMessage)
        {
            if (ex == null || ex.Errors.Count == 0)
            {
                return false;
            }

            SqlError error = ex.Errors[0];

            return error.Number == errorNumber || ex.Message.Contains(errorMessage);
        }
    }
}
