using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// SQL App Lock class
    /// </summary>
    [Component]
    public class SqlAppLock : ISqlAppLock
    {
        private readonly DbConnection con;
        private readonly string name;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlAppLock(DbConnection con, string name) : this(con, name, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlAppLock(DbConnection con, string name, TimeSpan wait)
        {
            this.con = con;
            this.name = name;
            LockAcquired = AcquireLock(wait);
        }

        /// <summary>
        /// Was the lock acquired?
        /// </summary>
        public bool LockAcquired { get; set; }

        /// <summary>
        /// Acquire a session, exclusive lock with the specified name.  If the lock cannot be obtained,
        /// the method waits for the specified time before returning false.
        /// </summary>
        private bool AcquireLock(TimeSpan wait)
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

            return result >= 0;
        }

        /// <summary>
        /// Release the lock previously taken
        /// </summary>
        private void ReleaseLock()
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

        /// <summary>
        /// Dispose and release the SQL app lock
        /// </summary>
        public void Dispose()
        {
            ReleaseLock();
        }
    }
}
