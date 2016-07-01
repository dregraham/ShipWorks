using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.Interaction
{
    /// <summary>
    /// Provides communications between background operations and components that need to know
    /// when they are active.  Such as the UI not being able to log off when a download is happening.
    /// </summary>
    public static class ApplicationBusyManager
    {
        static Dictionary<ApplicationBusyToken, string> activeOperations = new Dictionary<ApplicationBusyToken, string>();

        /// <summary>
        /// List of unique active operations.  If two operations are active that are the same, only one will be
        /// returned.
        /// </summary>
        public static List<string> GetActiveOperations()
        {
            List<string> operations = new List<string>();

            lock (activeOperations)
            {
                foreach (string value in activeOperations.Values)
                {
                    if (!operations.Contains(value))
                    {
                        operations.Add(value);
                    }
                }
            }

            return operations;
        }

        /// <summary>
        /// Wait for any pending background operations to complete.  Returns true if none are running, or false if they still are.
        /// </summary>
        public static bool WaitForOperations(IWin32Window owner, string uiUserGoalText, Action acquireAction)
        {
            Debug.Assert(!Program.ExecutionMode.IsUISupported || !Program.MainForm.InvokeRequired);

            lock (activeOperations)
            {
                if (activeOperations.Count == 0)
                {
                    acquireAction();

                    return true;
                }
            }

            if (CrashDialog.IsApplicationCrashed)
            {
                return false;
            }

            if (Program.ExecutionMode.IsUISupported)
            {
                using (ApplicationBusyDlg dlg = new ApplicationBusyDlg(uiUserGoalText))
                {
                    if (dlg.ShowDialog(owner) == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }
            else
            {
                Stopwatch timer = Stopwatch.StartNew();

                // Try for 10 seconds for there to be no more
                while (timer.Elapsed < TimeSpan.FromSeconds(10))
                {
                    if (GetActiveOperations().Count == 0)
                    {
                        break;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                }
            }

            lock (activeOperations)
            {
                if (activeOperations.Count == 0)
                {
                    acquireAction();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Indicate that the given operation is starting and now in progress.  The token returned
        /// should be passed to OperationComplete when the opeation is finished. If the operation cannot be started due to ConnextionSenstiveScope already
        /// being active, an exception is thrown.
        /// </summary>
        public static ApplicationBusyToken OperationStarting(string uiOperationText)
        {
            ApplicationBusyToken token;
            if (!TryOperationStarting(uiOperationText, out token))
            {
                throw new InvalidOperationException("Cannot start a background operation while within a connection sensitive scope.");
            }

            return token;
        }

        /// <summary>
        /// Indicate that the given operation is starting and now in progress.  The token returned
        /// should be passed to OperationComplete when the opeation is finished.  If the operation cannot be started due to ConnextionSenstiveScope already
        /// being active, false is returned.
        /// </summary>
        public static bool TryOperationStarting(string uiOperationText, out ApplicationBusyToken token)
        {
            lock (activeOperations)
            {
                if (ConnectionSensitiveScope.IsActive)
                {
                    token = null;

                    return false;
                }
                else
                {
                    token = new ApplicationBusyToken();
                    activeOperations[token] = uiOperationText;

                    return true;
                }
            }
        }

        /// <summary>
        /// Mark the specified background operation as complete.
        /// </summary>
        public static void OperationComplete(ApplicationBusyToken operationToken)
        {
            if (operationToken == null)
            {
                throw new ArgumentNullException("operationToken");
            }

            lock (activeOperations)
            {
                activeOperations.Remove(operationToken);
            }
        }
    }
}
