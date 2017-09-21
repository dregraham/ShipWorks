using log4net;
using ShipWorks.Data.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace ShipWorks.Tests.Data.Connection
{
    public class ConnectionMonitorExceptionLoggerTest
    {
        [Fact]
        public void Log_Works_WhenException()
        {
            StringBuilder msg = new StringBuilder();
            ILog log = LogManager.GetLogger(typeof(ConnectionMonitor));
            string testMsg = "This is a test exception";

            Exception ex = new Exception(testMsg);

            ConnectionMonitorExceptionLogger testObject = new ConnectionMonitorExceptionLogger();
            testObject.Log(ex, msg, log);

            Assert.True(msg.ToString().Contains(testMsg));
        }

        [Fact]
        public void Log_Works_WhenExceptionHasInnerException()
        {
            StringBuilder msg = new StringBuilder();
            ILog log = LogManager.GetLogger(typeof(ConnectionMonitor));
            string testMsg = "This is a test exception";
            string innerMsg = "This is an inner exception";

            Exception ex = new Exception(testMsg, new Exception(innerMsg));

            ConnectionMonitorExceptionLogger testObject = new ConnectionMonitorExceptionLogger();
            testObject.Log(ex, msg, log);

            Assert.True(msg.ToString().Contains(testMsg));
            Assert.True(msg.ToString().Contains(innerMsg));
        }

        [Fact]
        public void Log_Works_WhenTransactionScopeActive()
        {
            StringBuilder msg = new StringBuilder();
            ILog log = LogManager.GetLogger(typeof(ConnectionMonitor));
            string testMsg = "This is a test exception";
            string innerMsg = "This is an inner exception";

            using (TransactionScope scope = new TransactionScope())
            {
                Exception ex = new Exception(testMsg, new Exception(innerMsg));

                ConnectionMonitorExceptionLogger testObject = new ConnectionMonitorExceptionLogger();
                testObject.Log(ex, msg, log);
            }

            Assert.True(msg.ToString().Contains(testMsg));
            Assert.True(msg.ToString().Contains(innerMsg));
            Assert.True(msg.ToString().Contains("TransactionInfo:"));
        }

        [Fact]
        public void Log_Works_WhenCOMException()
        {
            StringBuilder msg = new StringBuilder();
            ILog log = LogManager.GetLogger(typeof(ConnectionMonitor));
            string testMsg = "This is a test exception";

            COMException ex = new COMException(testMsg, 999);

            ConnectionMonitorExceptionLogger testObject = new ConnectionMonitorExceptionLogger();
            testObject.Log(ex, msg, log);

            Assert.True(msg.ToString().Contains(testMsg));
            Assert.True(msg.ToString().Contains("COMException ErrorCode: 999"));
        }
    }
}
