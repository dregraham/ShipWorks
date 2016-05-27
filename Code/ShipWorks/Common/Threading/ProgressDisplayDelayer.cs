using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.UI.Controls;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Utility class for delaying the display of a progress window until a certain time has elapsed
    /// </summary>
    class ProgressDisplayDelayer
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ProgressDisplayDelayer));

        ProgressDlg progressDlg;
        Control owner;

        bool behaveAsModal;
        List<KeystrokeBlocker> keystrokeBlockers = new List<KeystrokeBlocker>();

        // Tracks when the operation is done, helps to konw if we should show progress or not
        ManualResetEvent doneEvent;

        // Indicates if the ShowAfterCallback has been called
        bool callbackCalled = false;

        #region class KeystrokeBlocker

        class KeystrokeBlocker : NativeWindow
        {
            Control control;

            bool blockedKeyDown = false;

            /// <summary>
            /// Constructor
            /// </summary>
            public KeystrokeBlocker(Control control)
            {
                this.control = control;

                if (control.IsHandleCreated)
                {
                    AssignHandle(control.Handle);
                }

                control.HandleCreated += new EventHandler(this.OnHandleCreated);
                control.HandleDestroyed += new EventHandler(this.OnHandleDestroyed);
            }

            /// <summary>
            /// The handle of the owned control is being created
            /// </summary>
            void OnHandleCreated(object sender, EventArgs e)
            {
                // Window is now created, assign handle to NativeWindow.
                AssignHandle(((Control) sender).Handle);
            }

            /// <summary>
            /// The handle of the owned control is being destroyed
            /// </summary>
            void OnHandleDestroyed(object sender, EventArgs e)
            {
                // Window was destroyed, release hook.
                ReleaseHandle();
            }

            /// <summary>
            /// Cancel the blocking
            /// </summary>
            public void CancelBlocking()
            {
                control.HandleCreated -= new EventHandler(this.OnHandleCreated);
                control.HandleDestroyed -= new EventHandler(this.OnHandleDestroyed);

                control = null;

                ReleaseHandle();
            }

            /// <summary>
            /// Intercept messages
            /// </summary>
            protected override void WndProc(ref Message m)
            {
                bool blockMessage = false;

                // If we are in a KEYDOWN, we want to let the pair complete, so we check that weve already blocked a
                // corresponding keydown before blocking a keyup
                if (m.Msg == NativeMethods.WM_KEYUP && blockedKeyDown)
                {
                    blockMessage = true;
                }

                if (m.Msg == NativeMethods.WM_KEYDOWN)
                {
                    blockedKeyDown = true;
                    blockMessage = true;
                }

                if (blockMessage)
                {
                    log.InfoFormat("Blocking message {0:X}", m.Msg);
                }
                else
                {
                    base.WndProc(ref m);
                }
            }

        }

        #endregion

        /// <summary>
        /// Create a new instance of the display delayer for the given progress window
        /// </summary>
        public ProgressDisplayDelayer(ProgressDlg progressDlg)
        {
            if (progressDlg == null)
            {
                throw new ArgumentNullException("progressDlg");
            }

            Debug.Assert(!progressDlg.AutoCloseWhenComplete, "The delayer should be responsible for closing the ProgressDlg.");
            progressDlg.AutoCloseWhenComplete = false;
            progressDlg.AllowCloseWhenComplete = false;

            this.progressDlg = progressDlg;
        }

        /// <summary>
        /// The ProgressDlg that this delayer is showing.
        /// </summary>
        public ProgressDlg ProgressDlg
        {
            get { return progressDlg; }
        }

        /// <summary>
        /// Show the progress dialog with the specified parent if the operation has not
        /// completed in the specified time.
        /// </summary>
        public void ShowAfter(Control owner, TimeSpan timeSpan)
        {
            ShowAfter(owner, timeSpan, true);
        }

        /// <summary>
        /// Show the progress dialog with the specified parent if the operation has not
        /// completed in the specified time. If behaveAsModal is true (the default) then the owner window is disabled
        /// while the progress is shown, even though the method does return immediately either way.
        /// </summary>
        public void ShowAfter(Control owner, TimeSpan timeSpan, bool behaveAsModal)
        {
            if (doneEvent != null)
            {
                throw new InvalidOperationException("ShowAfter is already in progress.");
            }

            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }

            if (owner.TopLevelControl != null)
            {
                owner = owner.TopLevelControl;
            }

            if (owner.InvokeRequired)
            {
                throw new InvalidOperationException("Must be called on the UI thread.");
            }

            this.behaveAsModal = behaveAsModal;
            this.owner = owner;
            this.doneEvent = new ManualResetEvent(false);

            // The UI should be disabled while waiting for the progress to show. We don't just wait
            // because the worker threads may need the UI pumping to get their work done.  But we have
            // to disable the UI so the user can't do stuff in between now and the progress being shown.
            if (behaveAsModal)
            {
                // The windows keyboard model is asynchronous, so its possible the user has a few keys in the input queue
                // that havn't come through yet (if you hit them real fast), but still will even after we are disabled.  We need to make sure we ignore
                // those keys, since we are supposed to be disabled.  I saw this as a problem firsthand while flipping through templates
                // in the TemplateManager very quickly.
                ActivateKeyboardBlocking(owner);

                if (owner != null && owner.IsHandleCreated)
                {
                    // Simulate modal by disabling the owner
                    NativeMethods.EnableWindow(owner.Handle, false);
                }
            }

            // Show the window after
            ThreadPool.RegisterWaitForSingleObject(doneEvent, new WaitOrTimerCallback(ShowAfterCallback), null, timeSpan, true);
        }

        /// <summary>
        /// Called when the work is done, or the waiting has timed out and we need to show the window.
        /// </summary>
        private void ShowAfterCallback(object state, bool timedOut)
        {
            // Have to get back on to the UI thread
            if (owner.InvokeRequired)
            {
                // can only call BeginInvoke if handle exists, which isn't true if timing is just right and the owner was closed/destroyed.
                if (owner.IsHandleCreated)
                {
                    owner.BeginInvoke(new WaitOrTimerCallback(ShowAfterCallback), state, timedOut);
                }
                else
                {
                    // Owner was closed, perform cleanup
                    ThreadPool.RegisterWaitForSingleObject(doneEvent, new WaitOrTimerCallback(CleanupAfterComplete), null, Timeout.Infinite, true);
                }

                return;
            }

            if (callbackCalled)
            {
                return;
            }
            else
            {
                callbackCalled = true;
            }

            bool isDone = !timedOut;

            // If it doesn't, show the window
            if (!isDone && Program.ExecutionMode.IsUISupported)
            {
                // This lock ensures that we wont show the window right after it really gets done, after having checked
                // right before that when it was not done.
                lock (doneEvent)
                {
                    isDone = doneEvent.WaitOne(0, false);

                    if (!isDone)
                    {
                        if (behaveAsModal)
                        {
                            CancelKeyboardBlocking();
                        }

                        if (!owner.IsDisposed)
                        {
                            progressDlg.BehaveAsModal = behaveAsModal;
                            progressDlg.Show(owner);
                        }
                    }
                }
            }

            // Close the handle when it becomes signaled
            ThreadPool.RegisterWaitForSingleObject(doneEvent, new WaitOrTimerCallback(CleanupAfterComplete), null, Timeout.Infinite, true);
        }

        /// <summary>
        /// Notify the delayer that the operation is about to complete.  This should be called before any attempts to close the
        /// progress window.
        /// </summary>
        public void NotifyComplete()
        {
            if (owner.InvokeRequired)
            {
                throw new InvalidOperationException("This should be called on the UI thread right before ending the ProgressDlg visibility.");
            }

            if (IsComplete)
            {
                throw new InvalidOperationException("The delayer is already in the complete state.");
            }

            lock (doneEvent)
            {
                // If the progress window is open, it will enable the UI when it closes
                if (progressDlg.Visible)
                {
                    progressDlg.CloseForced();
                }
                else
                {
                    if (behaveAsModal)
                    {
                        if (!owner.IsDisposed)
                        {
                            NativeMethods.EnableWindow(owner.Handle, true);
                        }

                        CancelKeyboardBlocking();
                    }
                }

                doneEvent.Set();
            }
        }

        /// <summary>
        /// Ensures that the ProgressDlg has been shown before returning.
        /// </summary>
        public void EnsureShown()
        {
            if (owner.InvokeRequired)
            {
                throw new InvalidOperationException("This should be called on the UI thread right before ending the ProgressDlg visibility.");
            }

            if (IsComplete)
            {
                throw new InvalidOperationException("The delayer is already in the complete state.");
            }

            // Force the callback to be called.  We'll ignore it when its called for real.
            if (!progressDlg.Visible)
            {
                ShowAfterCallback(null, true);
            }
        }

        /// <summary>
        /// Indicates if the delayer has already been marked as complete
        /// </summary>
        public bool IsComplete
        {
            get
            {
                return doneEvent == null;
            }
        }

        /// <summary>
        /// Cleanup our even after its completion
        /// </summary>
        private void CleanupAfterComplete(object state, bool timedOut)
        {
            doneEvent.Close();
            doneEvent = null;
        }

        /// <summary>
        /// Initialize blocking of keystroke messages for the given control and all child controls
        /// </summary>
        private void ActivateKeyboardBlocking(Control control)
        {
            if (control is InfoTip)
            {
                return;
            }

            keystrokeBlockers.Add(new KeystrokeBlocker(control));

            foreach (Control child in control.Controls)
            {
                ActivateKeyboardBlocking(child);
            }
        }

        /// <summary>
        /// Release keystroke blocking for all controls previously blocked.
        /// </summary>
        private void CancelKeyboardBlocking()
        {
            foreach (KeystrokeBlocker blocker in keystrokeBlockers)
            {
                blocker.CancelBlocking();
            }

            keystrokeBlockers.Clear();
        }
    }
}
