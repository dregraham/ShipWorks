using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Audit;
using ShipWorks.ApplicationCore;
using ShipWorks.Users;
using System.Transactions;
using ShipWorks.ApplicationCore.Logging;
using log4net;
using System.Diagnostics;
using ShipWorks.Data;

namespace ShipWorks.Tests
{
    /// <summary>
    /// Utility class for working with the database within unit tests
    /// </summary>
    public static class DatabaseManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(DatabaseManager));

        static Guid instanceID = new Guid("{5C3B2065-289F-4E16-B655-9894D133A086}");

        static SqlSessionScope sqlScope;
        static AuditBehaviorScope auditScope;
        static TransactionScope transactionScope;

        static long startupConnections = 0;
        static Stopwatch timer;

        /// <summary>
        /// Initialize UnitTests as if ShipWorks was active 
        /// </summary>
        public static void Initialize(string server, string database)
        {
            SqlSession sqlSession = new SqlSession();
            sqlSession.Configuration.ServerInstance = server;
            sqlSession.Configuration.DatabaseName = database;
            sqlSession.Configuration.WindowsAuth = true;

            sqlScope = new SqlSessionScope(sqlSession);
            auditScope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, AuditReason.Default, AuditBehaviorDisabledState.Disabled);

            ShipWorksSession.Initialize(instanceID);

            DataPath.Initialize();
            LogSession.Initialize();

            UserSession.InitializeForCurrentDatabase();
            UserManager.InitializeForCurrentUser();
            UserSession.InitializeForCurrentSession();

            transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5));

            StartTiming(false);
        }

        /// <summary>
        /// Start timing and tracking connections
        /// </summary>
        public static void StartTiming(bool logConnectionCallstacks)
        {
            // Clear the EntityCache
            DataProvider.InitializeForCurrentDatabase();

            startupConnections = ConnectionMonitor.TotalConnectionCount;
            timer = Stopwatch.StartNew();

            ConnectionMonitor.LogConnectionCallstacks = logConnectionCallstacks;
        }

        /// <summary>
        /// Cleanup and from what was initialized in the call to Initialize
        /// </summary>
        public static void Cleanup()
        {
            ConnectionMonitor.LogConnectionCallstacks = false;

            // Log total connections made
            Trace.WriteLine(string.Format("Total Connections (used by test(s)): {0}", ConnectionMonitor.TotalConnectionCount - startupConnections));
            Trace.WriteLine(string.Format("Total time (in test): {0}s", timer.Elapsed.TotalSeconds));

            transactionScope.Dispose();
            auditScope.Dispose();
            sqlScope.Dispose();

            transactionScope = null;
            auditScope = null;
            sqlScope = null;
        }
    }
}
