using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;

namespace ShipWorks.SqlServer.Common.Data
{
    /// <summary>
    /// Utility class for acquiring and releasing SQL Server app locks.  This class is just
    /// a wrapp around sp_getapplock and sp_releaseapplock.
    /// </summary>
    public static class SqlAppLockUtility
    {
        /// <summary>
        /// Acquire a session, exclusive lock with the specified name.  Returns false immediately if the lock
        /// cannot be obtained.
        /// </summary>
        public static bool AcquireLock(SqlConnection con, string name)
        {
            return AcquireLock(con, name, TimeSpan.Zero);
        }

        /// <summary>
        /// Acquire a session, exclusive lock with the specified name.  If the lock cannot be obtained,
        /// the method waits for the specified time before returning false.
        /// </summary>
        public static bool AcquireLock(SqlConnection con, string name, TimeSpan wait)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getapplock";

            // Ensure we don't have a SQL timeout before the applock timeout
            cmd.CommandTimeout = Math.Max(30, (int) wait.TotalSeconds + 30);

            SqlParameter returnValue = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.AddWithValue("@Resource", name);
            cmd.Parameters.AddWithValue("@LockMode", "Exclusive");
            cmd.Parameters.AddWithValue("@LockOwner", "Session");
            cmd.Parameters.AddWithValue("@LockTimeout", (int) wait.TotalMilliseconds);

            cmd.ExecuteNonQuery();

            int result = Convert.ToInt32(returnValue.Value);

            if (result == -999)
            {
                throw new InvalidOperationException("SQL Server returns -999 from sp_getapplock.");
            }

            return result >= 0;
        }

        /// <summary>
        /// Release the lock previously taken with the specified name
        /// </summary>
        public static void ReleaseLock(SqlConnection con, string name)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_releaseapplock";

            SqlParameter returnValue = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.AddWithValue("@Resource", name);
            cmd.Parameters.AddWithValue("@LockOwner", "Session");

            cmd.ExecuteNonQuery();

            int result = Convert.ToInt32(returnValue.Value);

            if (result == -999)
            {
                throw new InvalidOperationException("SQL Server returns -999 from sp_releaseapplock.");
            }
        }

        /// <summary>
        /// Determines if the given resource name is currently locked.
        /// </summary>
        public static bool IsLocked(SqlConnection con, string name)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT APPLOCK_TEST('public', @Resource, @LockMode, @LockOwner)";

            cmd.Parameters.AddWithValue("@Resource", name);
            cmd.Parameters.AddWithValue("@LockMode", "Exclusive");
            cmd.Parameters.AddWithValue("@LockOwner", "Session");

            return ((int) cmd.ExecuteScalar()) != 1;
        }

        /// <summary>
        /// Gets a SqlCommand from the given connection that is locked using the specified lock name.
        /// </summary>
        /// <param name="connection">Connection to use for locking and to create the command</param>
        /// <param name="lockName">Name of the lock that should be used</param>
        /// <param name="commandMethod">Action that will be called with the locked command</param>
        public static void GetLockedCommand(SqlConnection connection, string lockName, Action<SqlCommand> commandMethod)
        {
            bool needsClosing = false;

            try
            {
                // Need to have an open connection for the duration of the lock acquisition/release
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    needsClosing = true;
                }

                // Try to get a lock, if possible
                if (!IsLocked(connection, lockName) && AcquireLock(connection, lockName))
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            commandMethod(command);
                        }
                    }
                    finally
                    {
                        ReleaseLock(connection, lockName);
                    }
                }
                else
                {
                    // Let the caller know that someone else already has this lock
                    throw new SqlLockException(lockName);
                }
            }
            finally
            {
                // If this method opened the command, close it
                if (needsClosing && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
                
        }
    }
}
