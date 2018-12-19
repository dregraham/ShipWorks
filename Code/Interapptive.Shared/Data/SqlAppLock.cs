using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reactive.Disposables;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// SQL App Lock class
    /// </summary>
    [Component]
    public class SqlAppLock : ISqlAppLock
    {
        /// <summary>
        /// Acquire a session, exclusive lock with the specified name.  If the lock cannot be obtained,
        /// the method waits for the specified time before returning false.
        /// </summary>
        public IDisposable Take(DbConnection connection, string name, TimeSpan wait)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("con");
            }

            if (connection.State != ConnectionState.Open)
            {
                throw new ArgumentException("The given connection is not open.");
            }

            DbCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_getapplock";

            // Ensure we don't have a SQL timeout before the applock timeout
            cmd.CommandTimeout = Math.Max(30, (int) wait.TotalSeconds + 30);

            DbParameter returnValue = cmd.AddParameter("@ReturnValue", DbType.Int32);
            returnValue.Direction = ParameterDirection.ReturnValue;

            cmd.AddParameterWithValue("@Resource", name);
            cmd.AddParameterWithValue("@LockMode", "Exclusive");
            cmd.AddParameterWithValue("@LockOwner", "Session");
            cmd.AddParameterWithValue("@LockTimeout", (int) wait.TotalMilliseconds);

            cmd.ExecuteNonQuery();

            int result = Convert.ToInt32(returnValue.Value);

            if (result == -999)
            {
                throw new InvalidOperationException("SQL Server returns -999 from sp_getapplock.");
            }

            return result >= 0 ?
                Disposable.Create(() => ReleaseLock(connection, name)) :
                Disposable.Empty;
        }

        /// <summary>
        /// Release the lock previously taken
        /// </summary>
        private void ReleaseLock(DbConnection con, string name)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            if (con.State != ConnectionState.Open)
            {
                throw new ArgumentException("The given connection is not open.");
            }

            DbCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_releaseapplock";

            DbParameter returnValue = cmd.AddParameter("@ReturnValue", DbType.Int32);
            returnValue.Direction = ParameterDirection.ReturnValue;

            cmd.AddParameterWithValue("@Resource", name);
            cmd.AddParameterWithValue("@LockOwner", "Session");

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Message.IndexOf("because it is not currently held.", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    throw;
                }

                // Just eat the exception if we can't find the lock.
                return;
            }

            int result = Convert.ToInt32(returnValue.Value);

            if (result == -999)
            {
                throw new InvalidOperationException("SQL Server returns -999 from sp_releaseapplock.");
            }
        }
    }
}
