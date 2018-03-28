using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Administration;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// An instance of this class should be created before opening a window or doing an action that could change the database connection, restore, or backup the database.
    /// After the constructor finishes, Acquired must be checked to determine if the action can proceed.
    /// </summary>
    public sealed class ConnectionSensitiveScope : IConnectionSensitiveScope
    {
        private static readonly IConnectionSensitiveScope emptyScope = new EmptyConnectionScope();
        bool acquired = false;
        bool restoreInitiated = false;
        SqlSessionConfiguration originalSqlConfig = null;

        static List<ConnectionSensitiveScope> scopeStack = new List<ConnectionSensitiveScope>();

        // A new scope is about to be entered, but has not yet been acquired
        static public event EventHandler Acquiring;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionSensitiveScope(string uiUserGoalText, IWin32Window owner)
        {
            Debug.Assert(!Program.ExecutionMode.IsUISupported || !Program.MainForm.InvokeRequired);

            if (scopeStack.Count == 0)
            {
                Acquiring?.Invoke(null, EventArgs.Empty);
            }

            // Have to wait for any pending operations.  Since we haven't pushed this scope on to the stack yet, that means
            // new operations can still come in while we are waiting.
            acquired = ApplicationBusyManager.WaitForOperations(owner, uiUserGoalText, () => scopeStack.Add(this));

            // If we acquired the scope, we need to track changes to the database during it
            if (acquired)
            {
                ShipWorksBackup.RestoreStarting += new EventHandler(OnRestoreStarting);

                originalSqlConfig = SqlSession.IsConfigured ? SqlSession.Current.Configuration : null;
            }
        }

        /// <summary>
        /// Indicates if execution is within a ConnectionSensitiveScope.
        /// </summary>
        public static bool IsActive
        {
            get { return scopeStack.Count > 0; }
        }

        /// <summary>
        /// Indicates if the scope was successfully entered. If false, the window or action that could potentially affect the data connection
        /// must not proceed.
        /// </summary>
        public bool Acquired
        {
            get { return acquired; }
        }

        /// <summary>
        /// Indicates if the database, or the database we are pointing to via SqlSession, has changed during this scope
        /// </summary>
        public bool DatabaseChanged
        {
            get
            {
                SqlSessionConfiguration currentSqlConfig = SqlSession.IsConfigured ? SqlSession.Current.Configuration : null;

                return originalSqlConfig != currentSqlConfig || restoreInitiated;
            }
        }

        /// <summary>
        /// Empty implementation of the connection sensitive scope
        /// </summary>
        public static IConnectionSensitiveScope Empty => emptyScope;

        /// <summary>
        /// A restore is starting
        /// </summary>
        private void OnRestoreStarting(object sender, EventArgs e)
        {
            restoreInitiated = true;
        }

        /// <summary>
        /// Close the scope
        /// </summary>
        public void Dispose()
        {
            ShipWorksBackup.RestoreStarting -= new EventHandler(OnRestoreStarting);

            // Clear the current scope
            scopeStack.Remove(this);

            GC.SuppressFinalize(this);
        }
    }
}
