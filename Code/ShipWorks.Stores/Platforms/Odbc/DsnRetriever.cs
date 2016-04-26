using System;
using System.Data;
using System.Text;

namespace ShipWorks.Stores.Platforms.Odbc
{

    /// <summary>
    /// Retrieves the list of Dsns.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DsnRetriever : IDsnRetriever
    {
        private short dsnNameLength;
        private short dsnDescLength;
        private readonly StringBuilder dsnName;
        private readonly StringBuilder dsnDesc;
        private readonly IntPtr sqlEnvHandle;

        private const int SqlAttrOdbcVersion = 200;
        private const int SqlOvOdbc3 = 3;
        private const int MaxDsnLength = 32;

        /// <summary>
        /// Initializes a new instance of the <see cref="DsnRetriever"/> class.
        /// </summary>
        public DsnRetriever()
        {
            dsnName = new StringBuilder(MaxDsnLength);
            dsnDesc = new StringBuilder(128);

            dsnNameLength = 0;
            dsnDescLength = 0;

            sqlEnvHandle = InitializeSqlEnvHandle();
        }

        /// <summary>
        /// Gets the name of the next DSN.
        /// </summary>
        /// <returns>
        /// Returns Next DSN Name. False if none.
        /// </returns>
        /// <exception cref="DataException">Error getting ODBC Data Sources</exception>
        public string GetNextDsnName()
        {
            short resultCode = Odbc32.SQLDataSources(sqlEnvHandle.Value, Odbc32.Direction.SQL_FETCH_FIRST,
                dsnName, (short) dsnName.Capacity, ref dsnNameLength,
                dsnDesc, (short) dsnDesc.Capacity, ref dsnDescLength);

            if (resultCode != Odbc32.SQL_SUCCESS && resultCode != Odbc32.SQL_NO_DATA)
            {
                throw new DataException("Error getting ODBC Data Sources");
            }

            string nextDsnName = string.Empty;
            if (resultCode == Odbc32.SQL_SUCCESS)
            {
                nextDsnName = dsnName.ToString();
            }

            return nextDsnName;
        }

        /// <summary>
        /// Initializes the SQL env handle.
        /// </summary>
        /// <exception cref="DataException">
        /// Could not allocate ODBC Environment handle
        /// or
        /// Could not setup ODBC Environment handle
        /// </exception>
        private static IntPtr InitializeSqlEnvHandle()
        {
            IntPtr sqlEnvHandle;

            short rc = Odbc32.SQLAllocHandle(Odbc32.HandleType.SQL_HANDLE_ENV, 0, out sqlEnvHandle);
            if (rc != Odbc32.SQL_SUCCESS)
            {
                throw new DataException("Could not allocate ODBC Environment handle");
            }

            rc = Odbc32.SQLSetEnvAttr(sqlEnvHandle, SqlAttrOdbcVersion, (IntPtr)SqlOvOdbc3, 0);
            if (rc != Odbc32.SQL_SUCCESS)
            {
                throw new DataException("Could not setup ODBC Environment handle");
            }

            return sqlEnvHandle;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="DataException">Could not free ODBC Environment handle</exception>
        public void Dispose()
        {
            if (sqlEnvHandle.IsValueCreated)
            {
                short rc = Odbc32.SQLFreeHandle(Odbc32.HandleType.SQL_HANDLE_ENV, sqlEnvHandle.Value);
                if (rc != Odbc32.SQL_SUCCESS)
                {
                    throw new DataException("Could not free ODBC Environment handle");
                }
            }
        }
    }
}
