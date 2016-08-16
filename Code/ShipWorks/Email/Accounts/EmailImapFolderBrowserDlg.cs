using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rebex.Net;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid;
using ShipWorks.Properties;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.UI;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Browse for an imap folder
    /// </summary>
    public partial class EmailImapFolderBrowserDlg : Form
    {
        EmailAccountEntity account;
        string selectedFolder = "";

        class FolderNode
        {
            public FolderNode() { Children = new List<FolderNode>(); }

            public string Name { get; set; }
            public ImapFolder Folder { get; set; }
            public List<FolderNode> Children { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailImapFolderBrowserDlg(EmailAccountEntity account)
        {
            InitializeComponent();

            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            this.account = account;
        }

        /// <summary>
        /// Gets \ sets the currently selected IMAP folder
        /// </summary>
        public string SelectedFolder
        {
            get
            {
                return selectedFolder;
            }
            set
            {
                selectedFolder = value;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            ok.Enabled = false;
            cancel.Enabled = false;

            Task<ImapFolderCollection> getFolderTask = Task.Factory.StartNew(() =>
                {
                    using (Imap imap = EmailUtility.LogonToImap(account))
                    {
                        return imap.GetFolderList("", ImapFolderListMode.All, true);
                    }
                });

            getFolderTask.ContinueWith(LoadImapFolders, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Load the imap folders from the result of the async task
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadImapFolders(Task<ImapFolderCollection> getFoldersTask)
        {
            Cursor = Cursors.Default;
            cancel.Enabled = true;

            if (getFoldersTask.Exception != null)
            {
                Exception actualEx = getFoldersTask.Exception.Flatten().InnerExceptions[0];

                sandGrid.EmptyText = "";
                MessageHelper.ShowError(this, "There was an error loading the folder list:\n\n" + actualEx.Message);

                return;
            }

            ImapFolderCollection folders = getFoldersTask.Result;

            // Create the folder hierarchy
            List<FolderNode> rootNodes = new List<FolderNode>();
            foreach (ImapFolder folder in folders)
            {
                AddFolder(rootNodes, folder);
            }

            // Sort it by name
            SortRecursive(rootNodes);

            // If there is an "Inbox", float it to the top
            FolderNode inboxNode = rootNodes.FirstOrDefault(n => string.Compare(n.Name, "Inbox", StringComparison.OrdinalIgnoreCase) == 0);
            if (inboxNode != null)
            {
                // Make it look pretty incase its actually "INBOX";
                inboxNode.Name = "Inbox";

                rootNodes.Remove(inboxNode);
                rootNodes.Insert(0, inboxNode);
            }

            // Create all the rows
            CreateGridRows(rootNodes, sandGrid.Rows);

            // Can only OK if rows
            ok.Enabled = sandGrid.Rows.Count > 0;

            // Expand the first level
            foreach (GridRow row in sandGrid.Rows)
            {
                row.Expanded = true;
            }

            // Try to select the desired selection
            GridRow desiredSelection = sandGrid.FlatRows.Cast<GridRow>().FirstOrDefault(r => ((ImapFolder) r.Tag).Name == selectedFolder);
            if (desiredSelection != null)
            {
                GridRow parent = desiredSelection.ParentRow;
                while (parent != null)
                {
                    parent.Expanded = true;
                    parent = parent.ParentRow;
                }

                desiredSelection.Selected = true;
                desiredSelection.EnsureVisible();
            }
            else
            {
                if (sandGrid.Rows.Count > 0)
                {
                    sandGrid.Rows[0].Selected = true;
                }
                else
                {
                    sandGrid.EmptyText = "No folders were found.";
                }
            }
        }

        /// <summary>
        /// Sort the given nodes, and each of their child node collections recursively
        /// </summary>
        private void SortRecursive(List<FolderNode> rootNodes)
        {
            rootNodes.Sort(new Comparison<FolderNode>((left, right) => string.Compare(left.Name, right.Name, StringComparison.Ordinal)));

            foreach (FolderNode child in rootNodes)
            {
                SortRecursive(child.Children);
            }
        }

        /// <summary>
        /// Add the given IMAP folder to our tree
        /// </summary>
        private static void AddFolder(List<FolderNode> parentNodes, ImapFolder folder)
        {
            // Assume it's a root level
            string[] hierarchy = new string[] { folder.Name };

            // But then see if we can split it into parts
            if (folder.Delimiter != null && folder.Delimiter.Length == 1)
            {
                hierarchy = folder.Name.Split(folder.Delimiter[0]);
            }

            List<FolderNode> currentParent = parentNodes;

            // Add in each folder in the hierarchy
            foreach (string level in hierarchy)
            {
                FolderNode node = EnsureFolder(currentParent, level, folder);
                currentParent = node.Children;
            }
        }

        /// <summary>
        /// Ensure a folder with the given name exists within the specified parent row collection
        /// </summary>
        private static FolderNode EnsureFolder(List<FolderNode> parentNodes, string name, ImapFolder folder)
        {
            FolderNode node = parentNodes.FirstOrDefault(n => string.Compare(n.Name, name, StringComparison.OrdinalIgnoreCase) == 0);

            if (node == null)
            {
                node = new FolderNode { Name = name, Folder = folder };
                parentNodes.Add(node);
            }

            return node;
        }

        /// <summary>
        /// Add the collection of nodes to the given row collection, recursively
        /// </summary>
        private void CreateGridRows(List<FolderNode> nodes, GridRowCollection rowCollection)
        {
            foreach (FolderNode node in nodes)
            {
                GridRow row = new GridRow(node.Name, Resources.folderclosed);
                row.Tag = node.Folder;

                rowCollection.Add(row);

                CreateGridRows(node.Children, row.NestedRows);
            }
        }

        /// <summary>
        /// OK'ing the final selection
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (sandGrid.SelectedElements.Count == 0)
            {
                MessageHelper.ShowInformation(this, "Please select a folder.");
                return;
            }

            ImapFolder folder = (ImapFolder) sandGrid.SelectedElements[0].Tag;

            if (!folder.IsSelectable)
            {
                MessageHelper.ShowError(this, string.Format("The folder '{0}' is not selectable.  This is most likely a limitation of your IMAP server.", folder.Name));
                return;
            }

            selectedFolder = folder.Name;

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Window is trying to be closed
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !cancel.Enabled;
        }
    }
}
