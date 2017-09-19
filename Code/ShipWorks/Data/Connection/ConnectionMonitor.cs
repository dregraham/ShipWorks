using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model;
using ShipWorks.UI;
using System.Runtime.InteropServices;
using System.Transactions;
using System.Text;
using ShipWorks.Users.Audit;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Used to check for and recover from a lost database connection.
    /// </summary>
    public static class ConnectionMonitor
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ConnectionMonitor));

        static long connections = 0;

        // Can't open more than one at once
        static object connectionLock = new object();

        // Currently trying to make a reconnection
        static volatile bool attemptingReconnect = false;

        // Once this goes true, no further connections can be made.
        static volatile bool connectionLost = false;

        // Synchronization between threads for Database Reconnecting
        static ManualResetEvent reconnectEvent = new ManualResetEvent(false);

        // https://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlerror.number(v=vs.110).aspx
        // https://msdn.microsoft.com/en-us/library/cc231199.aspx
        private static readonly Lazy<List<int>> connectionErrorNumbers = new Lazy<List<int>>(() => new List<int> {
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
            });

        /// <summary>
        /// List of connection error numbers
        /// </summary>
        public static List<int> ConnectionErrorNumbers
        {
            get
            {
                return connectionErrorNumbers.Value;
            }
        }

        /// <summary>
        /// The total number of connections made since ShipWorks started
        /// </summary>
        public static long TotalConnectionCount
        {
            get
            {
                return Interlocked.Read(ref connections);
            }
        }

        /// <summary>
        /// Should only be used in a debug scenario to figure out whos creating connections and why
        /// </summary>
        public static bool LogConnectionCallstacks
        {
            get;
            set;
        }

        /// <summary>
        /// The current status of the connection monitor
        /// </summary>
        public static ConnectionMonitorStatus Status
        {
            get
            {
                if (connectionLost)
                {
                    return ConnectionMonitorStatus.ConnectionLost;
                }

                if (attemptingReconnect)
                {
                    return ConnectionMonitorStatus.AttemptReconnect;
                }

                return ConnectionMonitorStatus.Normal;
            }
        }

        /// <summary>
        /// Time of the last reconnection
        /// </summary>
        public static DateTime LastReconnection { get; private set; }

        /// <summary>
        /// Used to check if any background threads have detected a database disconnect and
        /// are waiting to be reconnected.  This is called from the Heartbeat to pickup/reconnect
        /// background disconnects.
        /// </summary>
        public static void VerifyConnected()
        {
            if (Status == ConnectionMonitorStatus.ConnectionLost)
            {
                // get a new database connection, which will force the reconnect
                SqlSession.Current.OpenConnection();
            }
        }

        /// <summary>
        /// Handles reconnecting to the database
        /// </summary>
        private static void Reconnect(DbConnection con)
        {
            if (IsUIThread())
            {
                log.Info("UI Thread is about to reconnect.");
                ReconnectResult result = ConnectionLostDlg.Reconnect(con);

                if (result == ReconnectResult.Canceled)
                {
                    log.Warn("User has elected to exit ShipWorks");
                    Program.MainForm.Close();
                    return;
                }

                // out of the reconnect window, set global state
                lock (connectionLock)
                {
                    // no longer trying to reconnect
                    attemptingReconnect = false;

                    // reconnect wasn't successful, the connection is lost
                    if (result != ReconnectResult.Succeeded)
                    {
                        connectionLost = true;
                    }

                    LastReconnection = DateTime.Now;

                    // signal to all waiting threads that the reconnect process has completed.  Completion doesn't not indicate a reconnect,
                    // just that we are done trying
                    reconnectEvent.Set();

                    // throw the ConnectionLostException
                    if (connectionLost)
                    {
                        throw new ConnectionLostException("The database connection has been permanently lost.");
                    }
                }
            }
            else
            {
                BackgroundReconnect(con);
            }
        }

        /// <summary>
        /// Background thread reconnect - waits for the UI thread to detect DB disconnect and reconnects
        /// </summary>
        private static void BackgroundReconnect(DbConnection con)
        {
            log.InfoFormat("Background thread starting to wait for reconnect event.");

            // Wait for the UI thread to reconnect
            reconnectEvent.WaitOne();

            log.InfoFormat("Background thread, reconnect event received. Will try to connect now.");

            // close the connection in preparation of the retry
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

            // now re-try
            OpenConnection(con);
        }

        /// <summary>
        /// Responsible for opening connections, and attempting to gracefully handle lost connection to the database.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void OpenConnection(DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            if (CrashDialog.IsApplicationCrashed)
            {
                throw new ConnectionLostException("The application is being shutdown due to a crash.");
            }

            // keep track of whether or not we have released the lock
            bool releaseRequired = true;

            // complicated locking/unlocking behavior so we aren't using lock()
            Monitor.Enter(connectionLock);
            try
            {
                if (connectionLost)
                {
                    throw new ConnectionLostException("The database connection has been permanently lost.");
                }

                // Some other thread detected a disconnect so we must either wait (background thread) or show the reconnect window (foreground)
                if (attemptingReconnect)
                {
                    log.InfoFormat("Detected another thread is attempting or waiting for a reconnect.");

                    // release the shared lock
                    Monitor.Exit(connectionLock);
                    releaseRequired = false;

                    // Reconnect to the database
                    Reconnect(con);

                    // exit early because we reconnected OK
                    return;
                }

                // try the connection
                try
                {
                    con.Open();

                    // Prepare the connection for use
                    PrepareConnectionForUse(con);
                }
                catch (Exception ex) when (ex is TransactionPromotionException || ex is COMException)
                {
                    LogConnectionException(ex);
                    throw;
                }
                catch (SqlException ex)
                {
                    log.WarnFormat("ConnectionMonitor failed to open connection: {0}", ex.Message);

                    if (!Program.ExecutionMode.IsUIDisplayed)
                    {
                        log.Warn("Application is not UserInteractive, not attempting to reconnect.");
                        throw new ConnectionLostException("The background process has lost connection to the database.", ex);
                    }

                    // Indicates MSDTC could not be started - which means we tried to open a new connection while an existing
                    // connection was still open during a transaction scope.
                    if (ex.Number == 8501)
                    {
                        LogConnectionException(ex);
                        throw;
                    }

                    // If it's a login failure, or b\c our connection has gone away, we have to check to see if it is because we were locked out due to SINGLE_USER
                    if (ex.Number == 4060 || ex.Number == 233)
                    {
                        if (!SingleUserModeScope.IsActive)
                        {
                            bool isSingleUser = false;

                            try
                            {
                                SqlSession master = new SqlSession(SqlSession.Current);
                                master.Configuration.DatabaseName = "master";

                                using (DbConnection testConnection = DataAccessAdapter.CreateConnection(master.Configuration.GetConnectionString()))
                                {
                                    testConnection.Open();

                                    isSingleUser = SqlUtility.IsSingleUser(testConnection, SqlSession.Current.Configuration.DatabaseName);
                                }
                            }
                            catch (Exception textEx)
                            {
                                log.Error("Could not login to master to try to check for SINGLE_USER mode", textEx);
                            }

                            if (isSingleUser)
                            {
                                throw new SingleUserModeException();
                            }
                        }
                        else
                        {
                            Stopwatch timer = Stopwatch.StartNew();

                            // Since SingleUserModeScope is active, we are here b\c another connection stole it from us.  We'll try to steal it back as soon as we can.
                            // Basically we'll try as hard as we can for up to 30 seconds by default, or an overridden amount of time
                            while (timer.Elapsed < SingleUserModeScope.ReconnectTimeout)
                            {
                                try
                                {
                                    con.Open();

                                    // Prepare the connection for use
                                    PrepareConnectionForUse(con);

                                    log.InfoFormat("Recovered from losing SINGLE_USER in {0}s", timer.Elapsed.TotalSeconds);

                                    return;
                                }
                                catch (SqlException)
                                {
                                    Thread.Sleep(1);

                                    SqlConnection.ClearAllPools();
                                }
                            }

                            throw new SqlScriptException("There was a problem maintaining the connection to the database.  Please try closing ShipWorks on your other computers.");
                        }
                    }

                    log.Info("Unable to connect, preparing to go into Reconnect Mode.");

                    // Now in reconnect mode, make everyone else wait
                    reconnectEvent.Reset();
                    attemptingReconnect = true;

                    // exit the lock
                    Monitor.Exit(connectionLock);
                    releaseRequired = false;

                    try
                    {
                        Reconnect(con);
                    }
                    catch (ConnectionLostException ex2)
                    {
                        // let the original sql exception be bubbled up
                        throw new ConnectionLostException(ex2.Message, ex);
                    }
                }
            }
            finally
            {
                if (releaseRequired)
                {
                    Monitor.Exit(connectionLock);
                }
            }

            // track total connections
            Interlocked.Increment(ref connections);

            if (LogConnectionCallstacks)
            {
                Debug.WriteLine(new StackTrace().ToString());
            }
        }

        /// <summary>
        /// Log a connection exception
        /// </summary>
        private static void LogConnectionException(Exception ex)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine($"A {ex.GetType().Name} occurred while attempting ConnectionMonitor.OpenCommection.");

            msg.AppendLine("Stack:");
            msg.AppendLine(new StackTrace().ToString());

            ConnectionMonitorExceptionLogger logger = new ConnectionMonitorExceptionLogger();
            logger.Log(ex, msg, log);
        }

        /// <summary>
        /// Prepare the open connection for use
        /// </summary>
        private static void PrepareConnectionForUse(DbConnection con)
        {
            // Since we are pooling, it will open without actually connecting to the server.
            // We need to do this to validate we can actually connect.  This sees at least a 10x slow down
            // but its necessary for a good user experience on a lost connection.
            ValidateOpenConnection(con);

            // If there is a deadlock priority in scope, set it now
            if (SqlDeadlockPriorityScope.Current != null)
            {
                DbCommandProvider.ExecuteNonQuery(con, string.Format("SET DEADLOCK_PRIORITY {0}", SqlDeadlockPriorityScope.Current.DeadlockPriority));

                // To read it back...
                // "SELECT deadlock_priority FROM sys.dm_exec_sessions where session_id = @@SPID"
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static bool IsUIThread()
        {
            return (!Program.MainForm.InvokeRequired);
        }

        /// <summary>
        /// Validates that an open connection is actually open.  With connection pooling, a connection in the Open state
        /// may not actually have a real connection to the database if the database has gone down or the network has dropped.
        ///
        /// This also serves to reset the connection isolation level to READ COMMITTED for every connection, as the connection pool
        /// does not do this! http://support.microsoft.com/kb/309544
        ///
        /// </summary>
        public static void ValidateOpenConnection(DbConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandTimeout = 5;
            cmd.CommandText = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";
            DbCommandProvider.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Handle the case where the system may be coming down because the connection is gone.  Returns
        /// true if handled, false if the exception is irrelevant.
        /// </summary>
        public static bool HandleTerminatedConnection(Exception ex)
        {
            BackgroundException backgroundEx = ex as BackgroundException;
            if (backgroundEx != null)
            {
                ex = backgroundEx.ActualException;
            }

            if (ex is ConnectionLostException)
            {
                log.Info("Terminating due to lost connection.", ex);

                return true;
            }
            else if (IsDbConnectionException(ex) || ex is SingleUserModeException)
            {
                connectionLost = true;

                if (Program.ExecutionMode.IsUIDisplayed)
                {
                    // We have to show the UI on the UI thread.  Otherwise behavior is a little undefined.  The only problem would be
                    // if the UI thread is currently blocking waiting on a background operation that is waiting for the connection to come back.
                    // But as far as I know we don't do this anyhwere
                    Program.MainForm.Invoke(new MethodInvoker(() =>
                    {
                        if (ex is SingleUserModeException)
                        {
                            using (SingleUserModeDlg dlg = new SingleUserModeDlg())
                            {
                                dlg.ShowDialog(DisplayHelper.GetActiveForm());
                            }
                        }
                        else
                        {
                            using (ConnectionTerminatedDlg dlg = new ConnectionTerminatedDlg())
                            {
                                dlg.ShowDialog(DisplayHelper.GetActiveForm());
                            }
                        }
                    }));
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Handle the exception that may have occurred on a background thread.  If its an exception related
        /// to a lost connection, and its in the background, we have to rethrow it on the gui thread.
        /// </summary>
        public static Exception TranslateConnectionException(Exception ex)
        {
            lock (connectionLock)
            {
                // See if we already know the connection to be lost
                if (connectionLost)
                {
                    return new ConnectionLostException("The database connection has been permanently lost.", ex);
                }

                // See if its a connection lost exception and update our state
                if (IsDbConnectionException(ex) || ex is ConnectionLostException)
                {
                    connectionLost = true;
                }

                return null;
            }
        }

        /// <summary>
        /// Indicates if the exception is due to a failed sql connection
        /// </summary>
        public static bool IsDbConnectionException(Exception ex)
        {
            IEnumerable<Exception> exceptions = ex.GetAllExceptions();
            IEnumerable<SqlException> sqlExceptions = exceptions.OfType<SqlException>();
            
            if (sqlExceptions.Any())
            {
                List<int> errors = sqlExceptions.Select(e => e.Number).ToList();

                foreach (SqlErrorCollection errorCollection in sqlExceptions.Select(e => e.Errors))
                {
                    foreach (SqlError sqlError in errorCollection)
                    {
                        errors.Add(sqlError.Number);
                    }
                }

                if (errors.Intersect(ConnectionErrorNumbers).Any())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
