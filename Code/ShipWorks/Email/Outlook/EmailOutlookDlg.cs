using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Grid.Columns;
using Divelements.SandGrid;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Outlook
{
    /// <summary>
    /// The ShipWorks equivalent of Outlook.  Sent Items, Outbox, and Inbox (eventually)
    /// </summary>
    public partial class EmailOutlookDlg : Form
    {
        Folder initialFolder;

        /// <summary>
        /// The folders displayed by the Outlook window
        /// </summary>
        public enum Folder
        {
            SentItems = 0,
            Outbox = 1
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailOutlookDlg() : this(Folder.SentItems)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailOutlookDlg(Folder initialFolder)
        {
            InitializeComponent();

            this.initialFolder = initialFolder;

            // Manage the window positioning
            WindowStateSaver windowSaver = new WindowStateSaver(this);
            windowSaver.ManageSplitter(splitContainer);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            EmailOutboxControl outbox = new EmailOutboxControl();
            outbox.Initialize(this);

            EmailSentItemsControl sentItems = new EmailSentItemsControl();
            sentItems.Initialize(this);

            sandTree.Rows[(int) Folder.SentItems].Tag = sentItems;
            sandTree.Rows[(int) Folder.Outbox].Tag = outbox;

            foreach (GridRow row in sandTree.Rows)
            {
                Control control = (Control) row.Tag;
                control.Dock = DockStyle.Fill;
                control.Visible = false;

                splitContainer.Panel2.Controls.Add(control);
            }

            // Select the default
            sandTree.Rows[(int) initialFolder].Selected = true;
        }

        /// <summary>
        /// The selected mailbox has changed
        /// </summary>
        private void OnChangeMailbox(object sender, Divelements.SandGrid.SelectionChangedEventArgs e)
        {
            Control desired = null;

            // Get the desired control to take up the view space
            if (sandTree.SelectedElements.Count == 1)
            {
                desired = (Control) sandTree.SelectedElements[0].Tag;
            }

            // Add the new one that is now selected
            if (desired != null)
            {
                desired.Visible = true;
                desired.BringToFront();

                foreach (Control control in splitContainer.Panel2.Controls)
                {
                    if (control != desired)
                    {
                        control.Visible = false;
                    }
                }
            }
        }
    }
}
