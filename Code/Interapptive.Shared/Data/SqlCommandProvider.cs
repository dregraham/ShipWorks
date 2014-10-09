using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using log4net;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Centralized class for creating and executing sql commands
    /// </summary>
    public static class SqlCommandProvider
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
        /// Creates a new SqlCommand initialized with the given connection
        /// </summary>
        public static SqlCommand Create(SqlConnection con)
        {
            return Create(con, "");
        }

        /// <summary>
        /// Creates a new SqlCommand initialized with the given connection and command text
        /// </summary>
        public static SqlCommand Create(SqlConnection con, string commandText)
        {
            return Create(con, commandText, null);
        }

        /// <summary>
        /// Creates a new SqlCommand initialized with the given connection and command text
        /// </summary>
        public static SqlCommand Create(SqlConnection con, string commandText, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand();

            if (con != null)
            {
                cmd.Connection = con;
            }

            if (!string.IsNullOrEmpty(commandText))
            {
                cmd.CommandText = commandText;
            }

            cmd.CommandTimeout = (int)DefaultTimeout.TotalSeconds;
            cmd.Transaction = transaction;

            return cmd;
        }

        /// <summary>
        /// Executes a scalar query using the given connection and SQL command text.  The value of the first row of the first column of the result set is returned.
        /// </summary>
        public static object ExecuteScalar(SqlConnection con, string commandText)
        {
            return ExecuteScalar(Create(con, commandText));
        }

        /// <summary>
        /// Executes a scalar query on the given SQL command.  The value of the first row of the first column of the result set is returned.
        /// </summary>
        public static object ExecuteScalar(SqlCommand cmd)
        {
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Executes a scalar query on the given SQL command.  The value of the first row of the first column of the result set is returned.
        /// </summary>
        public static T ExecuteScalar<T>(SqlConnection con, string commandText) where T : struct
        {
            using (SqlCommand command = Create(con, commandText))
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
        public static int ExecuteNonQuery(SqlConnection con, string commandText)
        {
            return ExecuteNonQuery(Create(con, commandText));
        }

        /// <summary>
        /// Executes a non-query on the given connection and sql command text.  The number of affected rows is returned.
        /// </summary>
        public static int ExecuteNonQuery(SqlConnection con, string commandText, SqlTransaction transaction)
        {
            return ExecuteNonQuery(Create(con, commandText, transaction));
        }

        /// <summary>
        /// Executes the given non-query on the given sql command.  The number of affected rows is returned.
        /// </summary>
        public static int ExecuteNonQuery(SqlCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Builds a new SqlDataReader based on the given connection and command text
        /// </summary>
        public static SqlDataReader ExecuteReader(SqlConnection con, string commandText)
        {
            return ExecuteReader(Create(con, commandText));
        }

        /// <summary>
        /// Builds a new SqlDataReader based on the given SqlCommand
        /// </summary>
        public static SqlDataReader ExecuteReader(SqlCommand cmd)
        {
            return cmd.ExecuteReader();
        }
    }
}
