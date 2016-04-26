using System;
using System.Data;
using System.Text;

namespace ShipWorks.Stores.Platforms.Odbc
{

    /// <summary>
    /// Retrieves the list of Dsns.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DnsProvider : IDnsProvider
    {
        private short dsnNameLength;
        private short dsnDescLength;
        private readonly StringBuilder dsnName;
        private readonly StringBuilder dsnDesc;
        private IntPtr sqlEnvHandle;

        private const int SqlAttrOdbcVersion = 200;
        private const int SqlOvOdbc3 = 3;
        private const int MaxDsnLength = 32;
        private Odbc32.Direction direction = Odbc32.Direction.SQL_FETCH_FIRST;

        /// <summary>
        /// Initializes a new instance of the <see cref="DnsProvider"/> class.
        /// </summary>
        public DnsProvider()
        {
            dsnName = new StringBuilder(MaxDsnLength);
            dsnDesc = new StringBuilder(128);

            dsnNameLength = 0;
            dsnDescLength = 0;

            sqlEnvHandle = IntPtr.Zero;
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

            if (sqlEnvHandle == IntPtr.Zero)
            {
                InitializeSqlEnvHandle();
            }

            short resultCode = Odbc32.SQLDataSources(sqlEnvHandle, direction,
                dsnName, (short) dsnName.Capacity, ref dsnNameLength,
                dsnDesc, (short) dsnDesc.Capacity, ref dsnDescLength);
            
            if (resultCode != Odbc32.SQL_SUCCESS && resultCode != Odbc32.SQL_NO_DATA)
            {
                throw new DataException("Error getting ODBC Data Sources");
            }

            string nextDsnName = null;
            if (resultCode == Odbc32.SQL_SUCCESS)
            {
                nextDsnName = dsnName.ToString();
            }

            direction = Odbc32.Direction.SQL_FETCH_NEXT;

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
        private void InitializeSqlEnvHandle()
        {
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
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="DataException">Could not free ODBC Environment handle</exception>
        public void Dispose()
        {
            if (sqlEnvHandle != IntPtr.Zero)
            {
                short rc = Odbc32.SQLFreeHandle(Odbc32.HandleType.SQL_HANDLE_ENV, sqlEnvHandle);
                if (rc != Odbc32.SQL_SUCCESS)
                {
                    throw new DataException("Could not free ODBC Environment handle");
                }

                sqlEnvHandle = IntPtr.Zero;
            }
        }
    }
}
