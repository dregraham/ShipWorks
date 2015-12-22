using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.ExecutionMode;
using log4net;
using System.Threading;
using ThreadTimer = System.Threading.Timer;
using System.Linq;
using ShipWorks.Users;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Common.Threading;
using Interapptive.Shared.Win32;
using ShipWorks.Data.Administration;
using System.Data.SqlClient;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Performs necessary background work when the user is idle
    /// </summary>
    public static class IdleWatcher
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(IdleWatcher));

        // Time user must be away to be considered idle
        static TimeSpan awayThreshold = TimeSpan.FromMinutes(5);
        static TimeSpan pollFrequency = TimeSpan.FromMinutes(1);
        static TimeSpan minimumWorkStartInterval = TimeSpan.FromSeconds(15);

        // Last time the mouse or keyboard was used in ShipWorks
        static DateTime lastUserInput = DateTime.UtcNow;

        // Idle state at last check
        static bool lastIdle = false;

        // Polling for idle
        static ThreadTimer timer;

        // Raised when entering\leaving idle state.  Can be called back on any thread.
        public static event EventHandler IdleChanged;

        // List of work that can be done any time there is any idle
        static List<IdleWork> idleWork = new List<IdleWork>();
        static int idleWorkIndex = 0;

        #region Internal Helper Classes

        /// <summary>
        /// Used to monitor for user input messages, so we know how long it has been
        /// since the last user input.
        /// </summary>
        class MessageFilter : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if ((m.Msg >= NativeMethods.WM_MOUSEFIRST && m.Msg <= NativeMethods.WM_MOUSELAST) ||
                    (m.Msg >= NativeMethods.WM_KEYFIRST && m.Msg <= NativeMethods.WM_KEYLAST))
                {
                    IdleWatcher.UserInputReceived();
                }

                return false;
            }
        }

        /// <summary>
        /// Used to keep track of work items that must be executed when the application is idle
        /// </summary>
        class IdleWork
        {
            Guid workID = Guid.NewGuid();

            string name;
            MethodInvoker workInvoker;

            bool databaseDependent;
            string uiOperationText;

            TimeSpan maxFrequency;
            DateTime lastRan = DateTime.MinValue;

            object isPendingInvokeLock = new object();
            bool isPendingInvoke = false;

            object isRunningLock = new object();
            bool isRunning = false;

            /// <summary>
            /// Creates a worker that is not dependent on the database
            /// </summary>
            public IdleWork(string name, MethodInvoker workInvoker, TimeSpan maxFrequency)
            {
                this.name = name;
                this.workInvoker = workInvoker;
                this.maxFrequency = maxFrequency;

                this.databaseDependent = false;
                this.uiOperationText = null;
            }

            /// <summary>
            /// Creates a worker that is dependent on the database.  The text is what the user will see
            /// as a part of the Background Operation window if the user tries to do something that would
            /// alter the database connection while the work is taking place.
            /// </summary>
            public IdleWork(string name, MethodInvoker workInvoker, string uiOperationText, TimeSpan maxFrequency)
            {
                this.name = name;
                this.workInvoker = workInvoker;
                this.maxFrequency = maxFrequency;

                this.databaseDependent = true;
                this.uiOperationText = uiOperationText;
            }

            /// <summary>
            /// A name used for logging purposes
            /// </summary>
            public string Name
            {
                get { return name; }
            }

            /// <summary>
            /// An identifier to uniquely identify this work
            /// </summary>
            public Guid WorkID
            {
                get { return workID; }
            }

            /// <summary>
            /// Indicates if its technically possible for the work to be ran with regards to the database state, 
            /// without considering the frequency and when it was last ran.
            /// </summary>
            public bool CanRun
            {
                get
                {
                    if (databaseDependent)
                    {
                        // If we aren't configured for a database at all, or there is a scope active in which the connection could change (like Database Setup wizard),
                        // or we know something is wrong with the connection - bail.
                        if (ConnectionSensitiveScope.IsActive || 
                            !UserSession.IsLoggedOn ||
                            ConnectionMonitor.Status != ConnectionMonitorStatus.Normal)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            /// <summary>
            /// Indicates if the work can currently be run.  Takes into consideration the database state and whether
            /// the work depends on the database, as well as when it was last run and the allowable frequency.
            /// </summary>
            public bool ReadyToRun
            {
                get
                {
                    // If we have been invoked to run on the UI thread - but havn't yet recieved that callback
                    // then we aren't ready, b\c we already being ran.
                    lock (isPendingInvokeLock)
                    {
                        if (isPendingInvoke)
                        {
                            return false;
                        }
                    }

                    if (lastRan + maxFrequency > DateTime.Now)
                    {
                        return false;
                    }

                    if (!CanRun)
                    {
                        return false;
                    }

                    return true;
                }
            }

            /// <summary>
            /// Run the idle work.  The work is put off on to a background thread.  The method returns immediately, before the work completes.
            /// </summary>
            public void Run()
            {
                bool isUiExecutionMode = Program.ExecutionMode is UserInterfaceExecutionMode;

                // If its DB dependant, we have to register with the app busy manager, which has to be done through the UI
                if (databaseDependent && isUiExecutionMode && Program.MainForm.InvokeRequired)
                {
                    lock (isPendingInvokeLock)
                    {
                        if (isPendingInvoke)
                        {
                            return;
                        }

                        isPendingInvoke = true;
                    }

                    Program.MainForm.BeginInvoke(new MethodInvoker(Run));
                    return;
                }

                lock (isPendingInvokeLock)
                {
                    isPendingInvoke = false;
                }

                if (!CanRun)
                {
                    return;
                }

                // If its already running do nothing
                lock (isRunningLock)
                {
                    if (isRunning)
                    {
                        return;
                    }

                    isRunning = true;

                    log.InfoFormat("IdleWork: Starting work '{0}'", Name);
                }

                ApplicationBusyToken operationToken = null;

                // We need to prevent ui switching db state using the background operation manager if the task requires the database
                if (databaseDependent)
                {
                    // If we can't start, then just get out
                    if (!ApplicationBusyManager.TryOperationStarting(uiOperationText, out operationToken))
                    {
                        Finish(operationToken);
                        return;
                    }

                    // If something changed that makes it so we can't run (like the user is now logged out), then get out
                    if (!CanRun)
                    {
                        Finish(operationToken);
                        return;
                    }
                }

                lastRan = DateTime.Now;

                ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncRun), operationToken);
            }

            /// <summary>
            /// The actual running of the idle work, takes place on background thread.
            /// </summary>
            private void AsyncRun(object state)
            {
                try
                {
                    // Do the work
                    workInvoker();
                }
                finally
                {
                    Finish(state as ApplicationBusyToken);
                }
            }

            /// <summary>
            /// Finish running
            /// </summary>
            private void Finish(ApplicationBusyToken operationToken)
            {
                // See if we have to signal an operation complete
                if (operationToken != null)
                {
                    ApplicationBusyManager.OperationComplete(operationToken);
                }

                lock (isRunningLock)
                {
                    isRunning = false;

                    log.InfoFormat("IdleWork: Finished work '{0}'", Name);
                }
            }
        }

        #endregion

        /// <summary>
        /// Initialize idle processing.  Called at application startup.
        /// </summary>
        public static void Initialize()
        {
            MessageFilter messageFilter = new MessageFilter();
            Application.AddMessageFilter(messageFilter);

            timer = new ThreadTimer(new TimerCallback(OnTimer), null, pollFrequency, pollFrequency);
        }

        /// <summary>
        /// Indicates if the usage state of ShipWorks is currently idle
        /// </summary>
        public static bool IsIdle
        {
            get
            {
                TimeSpan userAway = DateTime.UtcNow - lastUserInput;

                return userAway > awayThreshold;
            }
        }

        /// <summary>
        /// Register work that should be done on idle regardless of whether the database is connected.
        /// </summary>
        public static Guid RegisterDatabaseIndependentWork(string name, MethodInvoker workerMethod, TimeSpan maxFrequency)
        {
            lock (idleWork)
            {
                IdleWork work = new IdleWork(name, workerMethod, maxFrequency);
                idleWork.Add(work);

                return work.WorkID;
            }
        }

        /// <summary>
        /// Registers a worker that is dependent on the database.  The text is what the user will see
        /// as a part of the Background Operation window if the user tries to do something that would
        /// alter the database connection while the work is taking place.
        /// </summary>
        public static Guid RegisterDatabaseDependentWork(string name, MethodInvoker workerMethod, string uiOperationText, TimeSpan maxFrequency)
        {
            lock (idleWork)
            {
                IdleWork work = new IdleWork(name, workerMethod, uiOperationText, maxFrequency);
                idleWork.Add(work);

                return work.WorkID;
            }
        }

        /// <summary>
        /// Cancel the work with the given identifier and unregisters it from the IdleWatcher.  If it is currently running it won't be stopped, but 
        /// it won't be ran again.
        /// </summary>
        public static void CancelRegistration(Guid workID)
        {
            lock (idleWork)
            {
                // Find the work
                IdleWork work = idleWork.Where(w => w.WorkID == workID).SingleOrDefault();
                if (work != null)
                {
                    idleWork.Remove(work);
                }
            }
        }

        /// <summary>
        /// Timer event raised to check for idle change
        /// </summary>
        private static void OnTimer(object state)
        {
            while (true)
            {
                CheckIdleChanged();

                if (!IsIdle)
                {
                    break;
                }

                if (!RunNextIdleWork())
                {
                    break;
                }

                // This is so we don't run too many workers simultaneously right on top of each other.  We could make this better
                // by using an Event object to actually wait for the previous work to complete up to a certain timeout, that way
                // we wouldn't wait longer than we had to.  But this isn't bad, and keeps things a little spaced out.  If a work
                // took longer than then timeout, then it will run at the same time as the next work... but that's not terrible, and
                // perhaps desirable/necessary if one work took a reallly long time.
                Thread.Sleep(minimumWorkStartInterval);
            }
        }

        /// <summary>
        /// See if the idle state has changed
        /// </summary>
        private static void CheckIdleChanged()
        {
            bool newIdle = IsIdle;

            // See if it changed
            bool idleChanged = newIdle != lastIdle;

            // Update our last known value
            lastIdle = newIdle;

            if (idleChanged)
            {
                log.InfoFormat("IdleChanged {0}", newIdle);

                EventHandler handler = IdleChanged;
                if (handler != null)
                {
                    handler(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Called when the mouse or keyboard is used with ShipWorks
        /// </summary>
        private static void UserInputReceived()
        {
            lastUserInput = DateTime.UtcNow;

            CheckIdleChanged();

            // Put the timer off for another threshold amount
            timer.Change(pollFrequency + TimeSpan.FromSeconds(2), pollFrequency);
        }

        /// <summary>
        /// Run the next IdleWork that needs executed.  Returns true if any work was ready and was run, or false otherwise.
        /// </summary>
        private static bool RunNextIdleWork()
        {
            // This for loop ensures we only try once around the list, in the case that no work is ready.
            for (int i = 0; i < idleWork.Count; i++)
            {
                IdleWork work;
                
                lock (idleWork)
                {
                    // Ensure the index is in bounds
                    idleWorkIndex = idleWorkIndex % idleWork.Count;

                    // Get the current one, and increment for next time
                    work = idleWork[idleWorkIndex];
                    idleWorkIndex++;
                }

                if (work.ReadyToRun)
                {
                    work.Run();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Force the work associated with the given identifier to be run, even if its not yet time or its not currently an idle period.  If
        /// its database dependent and there is no database connection, it will howerver not be ran.
        /// </summary>
        public static void RunWorkNow(Guid workID)
        {
            // Find the work
            IdleWork work = null;

            lock (idleWork)
            {
                work = idleWork.Where(w => w.WorkID == workID).SingleOrDefault();
            }

            if (work == null)
            {
                throw new ArgumentException("No idle work with the given id was found.");
            }

            // If its not able to be ran, just forget it
            if (!work.CanRun)
            {
                return;
            }

            // Run the work
            work.Run();
        }
    }
}
