using System;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Common.IO.KeyboardShortcuts
{
    /// <summary>
    /// Manages the session of keyboard shortcuts
    /// </summary>
    public class ShortcutSession : IInitializeForCurrentUISession
    {
        private readonly IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar;
        private readonly IMainForm mainForm;
        private readonly KeyboardShortcutKeyFilter keyboardShortcutFilter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShortcutSession(IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar, 
            IMainForm mainForm, 
            Func<KeyboardShortcutKeyFilter> filterFactory)
        {
            keyboardShortcutFilter = filterFactory();

            this.windowsMessageFilterRegistrar = windowsMessageFilterRegistrar;
            this.mainForm = mainForm;
        }

        /// <summary>
        /// Adds keyboardShortcutFilter to application
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // We need to be on the UI thread for message pumps to work, so check
            // to see if an Invoke is required, and do so if it is.
            if (mainForm == null)
            {
                return;
            }

            if (mainForm.InvokeRequired)
            {
                mainForm.Invoke((Action) InitializeForCurrentSession, null);
                return;
            }

            windowsMessageFilterRegistrar.AddMessageFilter(keyboardShortcutFilter);
        }

        /// <summary>
        /// Removes keyboardShortcutFilter to application
        /// </summary>
        public void EndSession()
        {
            windowsMessageFilterRegistrar.RemoveMessageFilter(keyboardShortcutFilter);
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            EndSession();
        }
    }
}