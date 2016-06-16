using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Utility class for wrapping background thread functions for monitoring for unhandled exceptions
    /// </summary>
    public static class ExceptionMonitor
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ExceptionMonitor));

        /// <summary>
        /// Wrap the given call with exception handling that propogates the exception to the main UI thread.
        /// </summary>
        public static ThreadStart WrapThread(ThreadStart callback)
        {
            StackTrace stackTrace = new StackTrace(1);

            return () => { Wrapper(stackTrace, null, () => { callback(); }); };
        }

        /// <summary>
        /// Wrap the given call with exception handling that propogates the exception to the main UI thread.  If uiOperationText is non-null, an operation is registered with the ApplicationBusyManager
        /// using the given text that stays active until the callback completes.  If it is null, no ApplicationBusyToken is created.
        /// </summary>
        public static ParameterizedThreadStart WrapThread(ParameterizedThreadStart callback, string uiOperationText = null)
        {
            StackTrace stackTrace = new StackTrace(1);

            ApplicationBusyToken busyToken = null;

            if (!string.IsNullOrWhiteSpace(uiOperationText))
            {
                busyToken = ApplicationBusyManager.OperationStarting(uiOperationText);
            }

            return (object state) => { Wrapper(stackTrace, busyToken, () => { callback(state); }); };
        }

        /// <summary>
        /// Wrap the given call with exception handling that propogates the exception to the main UI thread.  If uiOperationText is non-null, an operation is registered with the ApplicationBusyManager
        /// using the given text that stays active until the callback completes.  If it is null, no ApplicationBusyToken is created.
        /// </summary>
        public static WaitCallback WrapWorkItem(WaitCallback callback, string uiOperationText = null)
        {
            StackTrace stackTrace = new StackTrace(1);

            ApplicationBusyToken busyToken = null;

            if (!string.IsNullOrWhiteSpace(uiOperationText))
            {
                busyToken = ApplicationBusyManager.OperationStarting(uiOperationText);
            }

            return (object state) => { Wrapper(stackTrace, busyToken, () => { callback(state); }); };
        }

        /// <summary>
        /// Main wrapper function for handling the exceptions
        /// </summary>
        private static void Wrapper(StackTrace invokingThreadTrace, ApplicationBusyToken busyToken, MethodInvoker callback)
        {
            try
            {
                callback();
            }
            catch (Exception ex)
            {
                if (!HandleException(ex, invokingThreadTrace))
                {
                    throw;
                }
            }
            finally
            {
                if (busyToken != null)
                {
                    busyToken.Dispose();
                    busyToken = null;
                }
            }
        }

        /// <summary>
        /// Handle an exception thrown from the background thread
        /// </summary>
        private static bool HandleException(Exception ex, StackTrace invokingThreadTrace)
        {
            if (Program.ExecutionMode?.IsUIDisplayed != true)
            {
                return false;
            }

            // If the main window has already died, forget it
            if (Program.MainForm.Disposing || !Program.MainForm.Visible)
            {
                return false;
            }

            // If invoke isn't required, we don't need to do anything
            if (!Program.MainForm.InvokeRequired)
            {
                return false;
            }

            // See if the connection monitor wants to handle it
            Exception updatedEx = ConnectionMonitor.TranslateConnectionException(ex);
            if (updatedEx != null)
            {
                ex = updatedEx;
            }

            log.Info("Rethrowing exception from background thread on GUI thread.", ex);

            Program.MainForm.BeginInvoke((MethodInvoker) delegate
            {
                throw new BackgroundException(ex, invokingThreadTrace);
            });

            return true;
        }
    }
}
