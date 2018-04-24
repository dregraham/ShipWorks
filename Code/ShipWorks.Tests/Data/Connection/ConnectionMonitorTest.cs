using Interapptive.Shared.Tests.Utility;
using ShipWorks.Data.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Tests.Data.Connection
{
    public class ConnectionMonitorTest
    {
        /*
                232,   // Win32: The pipe is being closed.
                233,   // Win32: No process is on the other end of the pipe.
                10053, // Win32: A transport-level error has occurred when sending the request to the server. (provider: TCP Provider, error: 0 - An established connection was aborted by the software in your host machine.)
                10054, // Win32: An existing connection was forcibly closed by the remote host.
                17142, // SQL: SQL Server service has been paused. No new connections will be allowed. To resume the service, use SQL Computer Manager or the Services application in Control Panel.
                64,    // Win32: The specified network name is no longer available
                -2,    // SQL: The wait operation timed out.
                258,   // Win32: The wait operation timed out.
                121,   // Win32: The semaphore time-out period has expired.
                109,   // Win32: The pipe has been ended.
                1236,  // Win32: The network connection was aborted by the local system.
                -1,    // SQL: A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found
                0,     // SQL: The connection is broken and recovery is not possible.  The connection is marked by the server as unrecoverable.  No attempt was made to restore the connection.
                596,   // SQL: Cannot continue the execution because the session is in the kill state
                59,    // SQL: A transport-level error has occurred when sending the request to the server. (provider: Named Pipes Provider, error: 0 - An unexpected network error occurred.)
                1130,  // Win32: A transport-level error has occurred when sending the request to the server. (provider: Named Pipes Provider, error: 0 - Not enough server storage is available to process this command.)
        */

        [Theory]
        [InlineData(232, true)]
        [InlineData(233, true)]
        [InlineData(10053, true)]
        [InlineData(10054, true)]
        [InlineData(64, true)]
        [InlineData(258, true)]
        [InlineData(121, true)]
        [InlineData(109, true)]
        [InlineData(1236, true)]
        [InlineData(1130, true)]
        public void IsDbConnectionException_ReturnsTrue_WhenSqlExceptionInnerExceptionMatches(int win32ExceptionNativeErrorNumber, bool expectedResult)
        {
            Win32Exception win32Exception = new Win32Exception(win32ExceptionNativeErrorNumber, "timeout");
            SqlException sqlException = SqlExceptionUtility.CreateSqlException(3, 3, win32Exception);

            Assert.Equal(expectedResult, ConnectionMonitor.IsDbConnectionException(sqlException));
        }

        [Fact]
        public void IsDbConnectionException_ReturnsTrue_WhenSqlExceptionNumberMatches()
        {
            foreach (int errorNumber in ConnectionMonitor.ConnectionErrorNumbers)
            {
                SqlException sqlException = SqlExceptionUtility.CreateSqlException(errorNumber, 3);

                Assert.True(ConnectionMonitor.IsDbConnectionException(sqlException));

                sqlException = SqlExceptionUtility.CreateSqlException(3, errorNumber);

                Assert.True(ConnectionMonitor.IsDbConnectionException(sqlException));
            }
        }

        [Fact]
        public void IsDbConnectionException_ReturnsFalse_WhenSqlExceptionNumberDoesNotMatche()
        {
            List<int> connectionErrors = new List<int> {
                5,
                -100
            };

            foreach (int errorNumber in connectionErrors)
            {
                Win32Exception win32Exception = new Win32Exception(3, "timeout");
                SqlException sqlException = SqlExceptionUtility.CreateSqlException(errorNumber, 3, win32Exception);

                Assert.False(ConnectionMonitor.IsDbConnectionException(sqlException));

                sqlException = SqlExceptionUtility.CreateSqlException(3, errorNumber);

                Assert.False(ConnectionMonitor.IsDbConnectionException(sqlException));
            }
        }

        [Fact]
        public void IsDbConnectionException_ReturnsFalse_WhenNotSqlException()
        {
            Exception ex = new Exception("not sql exception");

            Assert.False(ConnectionMonitor.IsDbConnectionException(ex));

            ex = new Exception("not sql exception", new Exception("not inner sql exception"));

            Assert.False(ConnectionMonitor.IsDbConnectionException(ex));
        }

        [Fact]
        public void IsDbConnectionException_ReturnsTrue_WhenInnerSqlExceptionNumberMatches()
        {
            foreach (int errorNumber in ConnectionMonitor.ConnectionErrorNumbers)
            {
                SqlException sqlException = SqlExceptionUtility.CreateSqlException(errorNumber, 3);
                Exception ex = new Exception("Outer exception", sqlException);

                Assert.True(ConnectionMonitor.IsDbConnectionException(sqlException));

                sqlException = SqlExceptionUtility.CreateSqlException(3, errorNumber);
                ex = new Exception("Outer exception", sqlException);

                Assert.True(ConnectionMonitor.IsDbConnectionException(sqlException));

                sqlException = SqlExceptionUtility.CreateSqlException(3, errorNumber);

                // Now when the SqlException is ex.InnerException.InnerException
                ex = new Exception("Outer outer exception", new Exception("Outer exception", sqlException));

                Assert.True(ConnectionMonitor.IsDbConnectionException(sqlException));
            }
        }
    }
}
