using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ShipWorks.Stores.Platforms.Odbc
{

    /// <summary>
    /// Retrieves the list of DSNs.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DsnProvider : IDsnProvider
    {
        private short dsnNameLength;
        private short dsnDescLength;
        private StringBuilder dsnName;
        private StringBuilder dsnDesc;
        private IntPtr sqlEnvHandle;

        private const int SqlAttrOdbcVersion = 200;
        private const int SqlOvOdbc3 = 3;
        private const int MaxDsnLength = 32;
        private Odbc32.Direction direction = Odbc32.Direction.SqlFetchFirst;

        /// <summary>
        /// Gets the available data sources.
        /// </summary>
        /// <exception cref="System.Data.DataException">
        /// Thrown when there is an issue retrieving information from the data sources.
        /// </exception>
        public IEnumerable<string> GetDataSourceNames()
        {
            try
            {
                List<string> odbcDataSources = new List<string>();
                string nextOdbcDataSource;

                InitializeSqlEnvHandle();

                // Continue adding DSNs to the list of data source names until there aren't any more
                while (!string.IsNullOrWhiteSpace(nextOdbcDataSource = GetNextDsn()))
                {
                    odbcDataSources.Add(nextOdbcDataSource);
                }

                return odbcDataSources;
            }
            finally
            {
                if (sqlEnvHandle != IntPtr.Zero)
                {
                    short rc = Odbc32.SQLFreeHandle(Odbc32.HandleType.SqlHandleEnv, sqlEnvHandle);
                    if (rc != Odbc32.SqlSuccess)
                    {
                        throw new DataException("Could not free ODBC Environment handle");
                    }

                    sqlEnvHandle = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Gets the name of the next DSN.
        /// </summary>
        /// <returns>
        /// Returns Next DSN Name; null if none.
        /// </returns>
        /// <exception cref="DataException">Error getting ODBC Data Sources</exception>
        private string GetNextDsn()
        {
            if (sqlEnvHandle == IntPtr.Zero)
            {
                InitializeSqlEnvHandle();
            }

            short resultCode = Odbc32.SQLDataSources(sqlEnvHandle, direction,
                dsnName, (short) dsnName.Capacity, ref dsnNameLength,
                dsnDesc, (short) dsnDesc.Capacity, ref dsnDescLength);

            if (resultCode != Odbc32.SqlSuccess && resultCode != Odbc32.SqlNoData)
            {
                throw new DataException("Error getting ODBC Data Sources");
            }

            string nextDsn = null;
            if (resultCode == Odbc32.SqlSuccess)
            {
                nextDsn = dsnName.ToString();
            }

            direction = Odbc32.Direction.SqlFetchNext;

            return nextDsn;
        }

        /// <summary>
        /// Initializes the SQL env handle.
        /// </summary>
        /// <exception cref="DataException">
        /// Could not allocate ODBC Environment handle
        /// or
        /// Could not setup ODBC Environment handle
        /// </exception>
        private void InitializeSqlEnvHandle()
        {
            dsnName = new StringBuilder(MaxDsnLength);
            dsnDesc = new StringBuilder(128);

            dsnNameLength = 0;
            dsnDescLength = 0;

            sqlEnvHandle = IntPtr.Zero;

            short rc = Odbc32.SQLAllocHandle(Odbc32.HandleType.SqlHandleEnv, 0, out sqlEnvHandle);
            if (rc != Odbc32.SqlSuccess)
            {
                throw new DataException("Could not allocate ODBC Environment handle");
            }

            rc = Odbc32.SQLSetEnvAttr(sqlEnvHandle, SqlAttrOdbcVersion, (IntPtr)SqlOvOdbc3, 0);
            if (rc != Odbc32.SqlSuccess)
            {
                throw new DataException("Could not setup ODBC Environment handle");
            }
        }
    }
}
