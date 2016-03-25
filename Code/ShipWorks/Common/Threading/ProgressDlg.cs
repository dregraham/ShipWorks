using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Autofac;
using Divelements.SandGrid;
using Divelements.SandGrid.Specialized;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Properties;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Window for displaying detailed progress updates.
    /// </summary>
    public partial class ProgressDlg : Form
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ProgressDlg));

        // Indicates if the user can close the window while the operation is in progress
        bool allowCloseWhenRunning = false;

        // Indicates if the user is allowed to close the window when the operation is done.  If false, then Close must
        // be explicitly called.
        bool allowCloseWhenComplete = true;

        // Indicates if the window closes by itself when finished. (Only if there are no errors).
        bool autoCloseWhenComplete = false;

        // Source of progress items to display
        ProgressProvider progressProvider;

        // What to show for the "OK" button, depending on state
        string closeTextWhenRunning = "OK";
        string closeTextWhenComplete = "OK";

        // Controls if the window behaves like a modal window even if Show and not ShowDialog is called.
        bool behaveAsModal = true;

        // Controls UI update rate, so tons of status messages don't flood us
        bool uiUpdatePending = false;
        object uiUpdateLock = new object();

        // Indiciates if the last known state of the progress provider was running.  This is just
        // so we don't update the header image too much, which screws up the animation.
        bool lastStateComplete = false;

        // Helps us with auto close on complete, to make sure we don't close automatically
        // before even starting
        bool hasStarted = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressDlg(ProgressProvider progressProvider)
        {
            InitializeComponent();

            if (progressProvider == null)
            {
                throw new ArgumentNullException("progressProvider");
            }

            // Get rid of the sample rows from the designer
            progressGrid.Rows.Clear();

            this.progressProvider = progressProvider;

            title.Text = "";
            description.Text = "";
        }

        /// <summary>
        /// Window is being loaded
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (!Modal)
            {
                // Center on the owner as if its our parent
                if (Owner != null && StartPosition == FormStartPosition.CenterParent)
                {
                    CenterWindow(Owner);
                }

                // Modal simulation
                if (Owner != null && BehaveAsModal)
                {
                    log.DebugFormat("Beginning simulated modal on owner.");
                    NativeMethods.EnableWindow(Owner.Handle, false);
                }
            }

            // Save window size
            WindowStateSaver wss = new WindowStateSaver(this, WindowStateSaverOptions.Size, "Progress - " + Title);

            // Load the rows
            LoadProgressItems();

            // We have to listen for changes to the progress rows
            progressProvider.ProgressItems.CollectionChanged += new CollectionChangedEventHandler<ProgressItem>(OnChangeProgressItems);
        }

        #region Properties

        /// <summary>
        /// The ProgressProvider the window is using for displaying progress.
        /// </summary>
        public ProgressProvider ProgressProvider
        {
            get { return progressProvider; }
        }

        /// <summary>
        /// Title of the progress control
        /// </summary>
        public string Title
        {
            get
            {
                return title.Text;
            }
            set
            {
                title.Text = value;
            }
        }

        /// <summary>
        /// Description shown under the Title
        /// </summary>
        public string Description
        {
            get
            {
                return description.Text;
            }
            set
            {
                description.Text = value;
            }
        }

        /// <summary>
        /// Header for the "Action" column
        /// </summary>
        [DefaultValue("Action")]
        public string ActionColumnHeaderText
        {
            get
            {
                return gridColumnAction.HeaderText;
            }
            set
            {
                gridColumnAction.HeaderText = value;
            }
        }

        /// <summary>
        /// Controls if the the window behaves like a modal window even if Show is called instead of ShowDialog.  This basically boils down
        /// to the fact that it disables the parent window while it is visible.
        /// </summary>
        [DefaultValue(true)]
        public bool BehaveAsModal
        {
            get
            {
                return behaveAsModal;
            }
            set
            {
                if (!DesignMode && Created)
                {
                    throw new InvalidOperationException("Cannot change Behavior setting after creation.");
                }

                behaveAsModal = value;
            }
        }

        /// <summary>
        /// Controls if the progress window can be closed before all the progress items are completd, canceled, or errored.
        /// </summary>
        [DefaultValue(false)]
        public bool AllowCloseWhenRunning
        {
            get
            {
                return allowCloseWhenRunning;
            }
            set
            {
                if (allowCloseWhenRunning == value)
                {
                    return;
                }

                allowCloseWhenRunning = value;

                UpdateProgressUI();
            }
        }

        /// <summary>
        /// Controls if the progress window can be closed by the user when complete, or if it has be done by a call to the Close method only.
        /// </summary>
        [DefaultValue(true)]
        public bool AllowCloseWhenComplete
        {
            get
            {
                return allowCloseWhenComplete;
            }
            set
            {
                if (allowCloseWhenComplete == value)
                {
                    return;
                }

                allowCloseWhenComplete = value;

                UpdateProgressUI();
            }
        }


        /// <summary>
        /// Indicates if the window closes by itself when finished. Only applies if there are no errors.
        /// </summary>
        [DefaultValue(false)]
        public bool AutoCloseWhenComplete
        {
            get
            {
                return autoCloseWhenComplete;
            }
            set
            {
                autoCloseWhenComplete = value;
            }
        }

        /// <summary>
        /// The text to display for the close button when the operation is running  By default it is "OK"
        /// </summary>
        [DefaultValue("OK")]
        public string CloseTextWhenRunning
        {
            get
            {
                return closeTextWhenRunning;
            }
            set
            {
                closeTextWhenRunning = value;

                UpdateProgressUI();
            }
        }

        /// <summary>
        /// The text to display for the close button when the operation is complete  By default it is "OK"
        /// </summary>
        [DefaultValue("OK")]
        public string CloseTextWhenComplete
        {
            get
            {
                return closeTextWhenComplete;
            }
            set
            {
                closeTextWhenComplete = value;

                UpdateProgressUI();
            }
        }

        #endregion

        #region Layout

        /// <summary>
        /// Center ourselves to the owner
        /// </summary>
        private void CenterWindow(Form owner)
        {
            Location = new Point(
                owner.Left + (owner.Width / 2 - Width / 2),
                owner.Top + (owner.Height / 2 - Height / 2));
        }

        #endregion

        #region Progress Item Rows

        /// <summary>
        /// A change to the collection of progress items
        /// </summary>
        void OnChangeProgressItems(object sender, CollectionChangedEventArgs<ProgressItem> e)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker) delegate { OnChangeProgressItems(sender, e); });
                return;
            }

            LoadProgressItems();
        }

        /// <summary>
        /// Sets the items that will have their progress reported
        /// </summary>
        public void LoadProgressItems()
        {
            if (InvokeRequired)
            {
                throw new InvalidOperationException("Cannot LoadProgressItems from non-UI thread.");
            }

            ClearRows();

            // Add each progress item
            foreach (ProgressItem item in progressProvider.ProgressItems)
            {
                // Create the grid row
                GridRow row = CreateBlankRow();
                row.Height = 0;
                row.Tag = item;

                progressGrid.Rows.Add(row);
                UpdateProgressRow(row);

                // Listen for changes
                item.Changed += new EventHandler(OnProgressItemChanged);
            }

            UpdateProgressUI();
        }

        /// <summary>
        /// Clear all the rows in the grid
        /// </summary>
        private void ClearRows()
        {
            // Go through each row in the grid and stop listening to the updates for the old ones
            foreach (GridRow row in progressGrid.Rows)
            {
                ((ProgressItem) row.Tag).Changed -= new EventHandler(OnProgressItemChanged);
            }

            // Reset the rows
            progressGrid.Rows.Clear();
        }

        #endregion

        #region Progress Display

        /// <summary>
        /// Create a new grid row with proper empty cells
        /// </summary>
        private GridRow CreateBlankRow()
        {
            GridRow row = new GridRow();
            row.Cells.Add(new GridCell());
            row.Cells.Add(new GridCell());
            row.Cells.Add(new GridCell());
            row.Cells.Add(new GridIntegerCell());

            return row;
        }

        /// <summary>
        /// A property of one of the ProgressItem objects we are listening on has changed
        /// </summary>
        void OnProgressItemChanged(object sender, EventArgs e)
        {
            // Not interested if we are either not created yet, or already destroyed
            if (!IsHandleCreated)
            {
                return;
            }

            lock (uiUpdateLock)
            {
                if (uiUpdatePending)
                {
                    return;
                }
                else
                {
                    uiUpdatePending = true;
                    EnableUpdateTimer();
                }
            }
        }

        /// <summary>
        /// Schedule an update of the UI
        /// </summary>
        private void EnableUpdateTimer()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(EnableUpdateTimer));
                return;
            }

            lock (uiUpdateLock)
            {
                if (uiUpdatePending)
                {
                    uiUpdateTimer.Enabled = true;
                }
            }
        }

        /// <summary>
        /// The timer to update the UI is being raised.
        /// </summary>
        private void OnUpdateTimer(object sender, EventArgs e)
        {
            lock (uiUpdateLock)
            {
                uiUpdatePending = false;
                uiUpdateTimer.Enabled = false;
            }

            foreach (GridRow gridRow in progressGrid.Rows)
            {
                UpdateProgressRow(gridRow);
            }

            // Update the whole ui
            UpdateProgressUI();
        }

        /// <summary>
        /// Update the UI of the overall window to act appropriatly based on the progress
        /// </summary>
        private void UpdateProgressUI()
        {
            // Not interested if we are either not created yet, or already destroyed
            if (!IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(UpdateProgressUI));
                return;
            }

            if (!hasStarted)
            {
                if (progressProvider.ProgressItems.Count == 0)
                {
                    ok.Enabled = false;
                    cancel.Enabled = false;
                }
                else
                {
                    hasStarted = true;
                }
            }

            if (hasStarted)
            {
                // Set the state of the cancel button
                cancel.Enabled = progressProvider.CanCancel;

                if (!progressProvider.IsComplete)
                {
                    if (lastStateComplete)
                    {
                        headerImage.Image = Resources.squares_circle_green;
                    }

                    ok.Enabled = allowCloseWhenRunning;
                    ok.Text = closeTextWhenRunning;
                }
                else
                {
                    ok.Enabled = allowCloseWhenComplete;
                    ok.Text = closeTextWhenComplete;

                    // Error overrides the cance\success images
                    if (progressProvider.HasErrors)
                    {
                        headerImage.Image = Resources.error32;
                    }

                    // Show the cancel UI
                    else if (progressProvider.CancelRequested)
                    {
                        headerImage.Image = Resources.cancelled32;

                    }
                    // Show the success UI
                    else
                    {
                        headerImage.Image = Resources.check32;
                    }

                    // See if we should auto close now that we are complete
                    if (autoCloseWhenComplete && !progressProvider.HasErrors)
                    {
                        BeginInvoke(new MethodInvoker(Close));
                    }
                }
            }

            // This is so we don't update the animiation image too much
            lastStateComplete = progressProvider.IsComplete;
        }

        /// <summary>
        /// Update the display of the given row
        /// </summary>
        private void UpdateProgressRow(GridRow row)
        {
            ProgressItem item = row.Tag as ProgressItem;

            if (item == null)
            {
                throw new InvalidOperationException("The Tag property of the GridRow was not a ProgressItem.");
            }

            if (InvokeRequired)
            {
                throw new InvalidOperationException("Cannot update progress display on non-UI thread.");
            }

            row.Cells[0].Image = GetStatusImage(item.Status);
            row.Cells[1].Text = item.Name;

            row.Cells[2].Text = item.Detail;
            row.Cells[2].ForeColor = (item.Status == ProgressItemStatus.Pending) ? Color.DarkGray : SystemColors.WindowText;

            if (item.PercentComplete > 0)
            {
                ((GridIntegerCell) row.Cells[3]).Value = item.PercentComplete;
            }
            else
            {
                row.Cells[3].IsNull = true;
            }

            // If its a failure, and has an error to display, display it
            if (item.Status == ProgressItemStatus.Failure && item.Error != null)
            {
                SandGridUtility.ShowNestedMessage(row, item.Error.Message, Color.Red);
            }

            // If its running, but we've been canceled, show the pending cancel message
            else if (progressProvider.CancelRequested && item.Status == ProgressItemStatus.Running)
            {
                SandGridUtility.ShowNestedMessage(row, "Canceling...", Color.OrangeRed);
            }

            // Otherwise, shouldnt be showing anything
            else
            {
                row.NestedRows.Clear();
            }
        }

        /// <summary>
        /// Get the image to use to display the given status
        /// </summary>
        private Image GetStatusImage(ProgressItemStatus status)
        {
            switch (status)
            {
                case ProgressItemStatus.Running: return Resources.arrow_right_green;
                case ProgressItemStatus.Success: return Resources.check16;
                case ProgressItemStatus.Failure: return Resources.error16;
                case ProgressItemStatus.Canceled: return Resources.cancel16;
                case ProgressItemStatus.Pending:
                default:
                    return null;
            }
        }

        #endregion

        #region Error Handling

        /// <summary>
        /// The context menu for copying error message is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            // Only show the menu when copying error text - which will be when an inner grid is the active grid
            if (progressGrid.PrimaryGrid == progressGrid.ActiveGrid)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Copy the selected error message
        /// </summary>
        private void OnCopyErrorMessage(object sender, EventArgs e)
        {
            string error = progressGrid.ActiveGrid.Rows[0].Cells[0].Text;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                lifetimeScope.Resolve<ClipboardHelper>().SetText(error, TextDataFormat.Text, null);
            }
        }

        #endregion

        #region Cancel \ Close

        /// <summary>
        /// User wants to cancel
        /// </summary>
        private void OnCancel(object sender, EventArgs e)
        {
            log.Info("User requested cancel.");

            cancel.Enabled = false;
            progressProvider.Cancel();

            UpdateProgressUI();
        }

        /// <summary>
        /// Force the window to close regardless of the AllowCloseXXX settings.  Calling the Form.Close function may not actually
        /// close the window if either of those settings where true.
        /// </summary>
        public void CloseForced()
        {
            allowCloseWhenComplete = true;
            allowCloseWhenRunning = true;

            Close();
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!progressProvider.IsComplete && !allowCloseWhenRunning)
            {
                e.Cancel = true;
                return;
            }

            if (progressProvider.IsComplete && !allowCloseWhenComplete)
            {
                e.Cancel = true;
                return;
            }

            if (!Modal && Owner != null && BehaveAsModal)
            {
                log.DebugFormat("Ending simulated modal on owner.");
                NativeMethods.EnableWindow(Owner.Handle, true);
            }

            // Don't care what happens to them now
            progressProvider.ProgressItems.CollectionChanged -= new CollectionChangedEventHandler<ProgressItem>(OnChangeProgressItems);

            ClearRows();
        }

        /// <summary>
        /// User has clicked the close button
        /// </summary>
        private void OnClose(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
