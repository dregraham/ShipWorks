using Interapptive.Shared.Tests.Utility;
using ShipWorks.Data.Connection;
using System;
using System.Collections.Generic;
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
                SqlException sqlException = SqlExceptionUtility.CreateSqlException(errorNumber, 3);

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
