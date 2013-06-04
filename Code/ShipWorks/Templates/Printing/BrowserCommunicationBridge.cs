using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Provides means for the browser to communicate what its status is, and to retrieve information for doing its job.
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class BrowserCommunicationBridge
    {
        // User associated data
        object tag;

        // Settings for the spooler to use
        BrowserSpoolerSettings spoolerSettings;

        // Page settings for the current template
        BrowserPageSettings pageSettings;

        // Contains information about the template results content the browser displays
        BrowserDocumentContent documentContent = new BrowserDocumentContent();

        // Used to load\save the preview window size
        BrowserWindowPosition windowPosition = new BrowserWindowPosition();

        /// <summary>
        /// Construct a new instance of a communication bridge
        /// </summary>
        internal BrowserCommunicationBridge(BrowserSpoolerSettings spoolerSettings, BrowserPageSettings pageSettings)
        {
            this.spoolerSettings = spoolerSettings;
            this.pageSettings = pageSettings;
        }

        /// <summary>
        /// The settings the spooler will use for the print job
        /// </summary>
        [Obfuscation(Exclude = true)]
        public BrowserSpoolerSettings SpoolerSettings
        {
            get { return spoolerSettings; }
        }

        /// <summary>
        /// The page settings for the template
        /// </summary>
        [Obfuscation(Exclude = true)]
        public BrowserPageSettings PageSettings
        {
            get { return pageSettings; }
            internal set { pageSettings = value; }
        }

        /// <summary>
        /// Contains information about the template results content the browser displays
        /// </summary>
        [Obfuscation(Exclude = true)]
        public BrowserDocumentContent DocumentContent
        {
            get { return documentContent; }
        }

        /// <summary>
        /// The window size of the preview window
        /// </summary>
        [Obfuscation(Exclude = true)]
        public BrowserWindowPosition WindowPosition
        {
            get { return windowPosition; }
        }

        /// <summary>
        /// User specific data
        /// </summary>
        internal object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// Raised when a print job has completed
        /// </summary>
        internal event EventHandler PrintingComplete;

        /// <summary>
        /// Raised when a preview has become visible to the user
        /// </summary>
        internal event EventHandler PreviewShown;

        /// <summary>
        /// Raised when the user has clicked the Print button to print the current preview
        /// </summary>
        internal event CancelEventHandler PreviewPrintNow;

        /// <summary>
        /// Raised when the user has closed the preview window without wanting to print
        /// </summary>
        internal event EventHandler PreviewCancel;

        /// <summary>
        /// Raised when the user wants to change the pring job settings
        /// </summary>
        internal event EventHandler ShowSettings;

        /// <summary>
        /// Raised when an error occurs during printing or preview.  No other events will be called after this during the print cycle.
        /// </summary>
        internal event BrowserErrorEventHandler Error;

        /// <summary>
        /// Called when the user successfully completes the print job.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyPrintingComplete()
        {
            if (PrintingComplete != null)
            {
                PrintingComplete(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called when the preview generation is complete and has been displayed to the user.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyPreviewReady()
        {
            if (PreviewShown != null)
            {
                PreviewShown(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// User has requested that the preview be printed
        /// </summary>
        [Obfuscation(Exclude = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool NotifyPreviewPrintNow()
        {
            CancelEventArgs args = new CancelEventArgs(false);

            if (PreviewPrintNow != null)
            {
                PreviewPrintNow(this, args);
            }

            return !args.Cancel;
        }

        /// <summary>
        /// User has closed the preview window
        /// </summary>
        [Obfuscation(Exclude = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyPreviewCancel()
        {
            if (PreviewCancel != null)
            {
                PreviewCancel(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// User wants to see the page settings window
        /// </summary>
        [Obfuscation(Exclude = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyShowSettings()
        {
            if (ShowSettings != null)
            {
                ShowSettings(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Called if the user attempted to print, but an error occurred.
        /// </summary>
        [Obfuscation(Exclude = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void NotifyError(string message, string line)
        {
            if (Error != null)
            {
                Error(this, new BrowserErrorEventArgs(message, line));
            }
        }
    }
}
