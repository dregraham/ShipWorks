using System;
using System.Data.Common;
using System.Globalization;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Centralized class for creating and executing sql commands
    /// </summary>
    public static class DbCommandProvider
    {
        // Match SQL Server default
        static TimeSpan defaultTimeout = TimeSpan.FromSeconds(30);

        /// <summary>
        /// The default command timeout
        /// </summary>
        public static TimeSpan DefaultTimeout
        {
            get { return defaultTimeout; }
            set { defaultTimeout = value; }
        }

        /// <summary>
        /// Creates a new DbCommand initialized with the given connection
        /// </summary>
        public static DbCommand Create(DbConnection con)
        {
            return Create(con, "");
        }

        /// <summary>
        /// Creates a new DbCommand initialized with the given connection and command text
        /// </summary>
        public static DbCommand Create(DbConnection con, string commandText)
        {
            return Create(con, commandText, null);
        }

        /// <summary>
        /// Creates a new DbCommand initialized with the given connection and command text
        /// </summary>
        public static DbCommand Create(DbConnection con, string commandText, DbTransaction transaction)
        {
            DbCommand cmd = con.CreateCommand();

            if (con != null)
            {
                cmd.Connection = con;
            }

            if (!string.IsNullOrEmpty(commandText))
            {
                cmd.CommandText = commandText;
            }

            cmd.CommandTimeout = (int) DefaultTimeout.TotalSeconds;
            cmd.Transaction = transaction;

            return cmd;
        }

        /// <summary>
        /// Executes a scalar query using the given connection and SQL command text.  The value of the first row of the first column of the result set is returned.
        /// </summary>
        public static object ExecuteScalar(DbConnection con, string commandText)
        {
            return ExecuteScalar(Create(con, commandText));
        }

        /// <summary>
        /// Executes a scalar query on the given SQL command.  The value of the first row of the first column of the result set is returned.
        /// </summary>
        public static object ExecuteScalar(DbCommand cmd)
        {
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Executes a scalar query on the given SQL command.  The value of the first row of the first column of the result set is returned.
        /// </summary>
        public static T ExecuteScalar<T>(DbConnection con, string commandText) where T : struct
        {
            using (DbCommand command = Create(con, commandText))
            {
                return ExecuteScalar<T>(command);
            }
        }

        /// <summary>
        /// Executes a scalar query on the given SQL command.  The value of the first row of the first column of the result set is returned.
        /// </summary>
        public static T ExecuteScalar<T>(DbCommand cmd) where T : struct
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }

            object result = cmd.ExecuteScalar();

            if (result == null)
            {
                throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "Could not convert null to {0}", typeof(T).Name));
            }

            if (result is T)
            {
                return (T) result;
            }

            throw new InvalidCastException(string.Format(CultureInfo.InvariantCulture, "Could not convert {0} to {1}", result.GetType().Name, typeof(T).Name));
        }

        /// <summary>
        /// Executes a non-query on the given connection and sql command text.  The number of affected rows is returned.
        /// </summary>
        public static int ExecuteNonQuery(DbConnection con, string commandText)
        {
            return ExecuteNonQuery(Create(con, commandText));
        }

        /// <summary>
        /// Executes a non-query on the given connection and sql command text.  The number of affected rows is returned.
        /// </summary>
        public static int ExecuteNonQuery(DbConnection con, string commandText, DbTransaction transaction)
        {
            return ExecuteNonQuery(Create(con, commandText, transaction));
        }

        /// <summary>
        /// Executes the given non-query on the given sql command.  The number of affected rows is returned.
        /// </summary>
        public static int ExecuteNonQuery(DbCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Builds a new DbDataReader based on the given connection and command text
        /// </summary>
        public static DbDataReader ExecuteReader(DbConnection con, string commandText)
        {
            return ExecuteReader(Create(con, commandText));
        }

        /// <summary>
        /// Builds a new DbDataReader based on the given DbCommand
        /// </summary>
        public static DbDataReader ExecuteReader(DbCommand cmd)
        {
            return cmd.ExecuteReader();
        }
    }
}
