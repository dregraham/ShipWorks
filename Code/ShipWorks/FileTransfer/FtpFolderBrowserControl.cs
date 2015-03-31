extern alias rebex2015;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using rebex2015::Rebex.IO;
using rebex2015::Rebex.Net;
using ShipWorks.Data.Model.EntityClasses;
using System.Threading.Tasks;
using Divelements.SandGrid;
using ShipWorks.Properties;
using Interapptive.Shared.UI;

namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// Control for browsing and selection of FTP folders
    /// </summary>
    public partial class FtpFolderBrowserControl : UserControl
    {
        const string rootName = "<home>";
        IFtp ftp;

        class FtpFolderNode
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public List<FtpFolderNode> Children { get; set; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FtpFolderBrowserControl()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (ftp != null)
                {
                    try
                    {
                        ftp.Disconnect();
                        ftp.Dispose();
                    }
                    catch
                    {

                    }

                    ftp = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Login to the FTP server and begin browsing using the specified account
        /// </summary>
        public void BeginInitialize(FtpAccountEntity account, string initialFolder, Action<Task> continuation)
        {
            sandGrid.Rows.Clear();
            sandGrid.EmptyText = "Loading...";

            Task<List<FtpFolderNode>> task = Task.Factory.StartNew(() =>
            {
                ftp = FtpUtility.LogonToFtp(account);

                // Since we get folders below root, we need to create a "fake" node so that
                // a user can select the root of an FTP site, if they want
                List<FtpFolderNode> rootFolders = new List<FtpFolderNode>
                {
                    new FtpFolderNode
                    {
                        Name = rootName,
                        Path = "/",
                        Children = GetFolderList("/")
                    }
                };

                if (!string.IsNullOrWhiteSpace(initialFolder))
                {
                    FetchNecessaryDecendants(rootFolders, rootName + EnsureStartingSlash(initialFolder));
                }

                return rootFolders;
            });

            task.ContinueWith(t => LoadRootFolders(t, initialFolder), TaskScheduler.FromCurrentSynchronizationContext())
                .ContinueWith(continuation, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Get or set the currently selected folder
        /// </summary>
        public string SelectedFolder
        {
            get
            {
                if (sandGrid.SelectedElements.Count > 0)
                {
                    FtpFolderNode node = (FtpFolderNode) sandGrid.SelectedElements[0].Tag;

                    return node.Path;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Ensure the passed string has a slash at the beginning
        /// </summary>
        private static string EnsureStartingSlash(string value)
        {
            if (value == null)
            {
                return "/";
            }

            return value.StartsWith("/", StringComparison.Ordinal) ? value : "/" + value;
        }

        /// <summary>
        /// Load the imap folders from the result of the async task
        /// </summary>
        private void LoadRootFolders(Task<List<FtpFolderNode>> task, string initialFolder)
        {
            sandGrid.EmptyText = "";

            if (task.Exception != null)
            {
                throw task.Exception;
            }

            List<FtpFolderNode> rootFolders = task.Result;

            CreateGridRows(rootFolders, sandGrid.Rows);

            // Try to select the desired selection
            string initialFolderWithSlash = EnsureStartingSlash(initialFolder);
            GridRow desiredSelection = sandGrid.FlatRows.Cast<GridRow>().FirstOrDefault(r => ((FtpFolderNode) r.Tag).Path == initialFolderWithSlash);

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

            if (sandGrid.Rows.Count > 0)
            {
                // Ensure the root row is expanded and not selected by default
                sandGrid.Rows[0].Expanded = true;

                // Select the default
                if (sandGrid.SelectedElements.Count == 0)
                {
                    sandGrid.Rows[0].Selected = true;
                }
            }
            else
            {
                sandGrid.EmptyText = "No folders were found.";
            }
        }

        /// <summary>
        /// Given the specified descendant path, starting from the current folder list, attempt to fill in all child folders through the specified descendant
        /// </summary>
        private void FetchNecessaryDecendants(List<FtpFolderNode> folders, string descendantPath)
        {
            var parts = descendantPath.Split('/').ToList();

            // The path should start with a "/", which will create an empty first part
            if (parts.Count > 0 && parts[0].Length == 0)
            {
                parts.RemoveAt(0);
            }

            // Once we hit one part left - we have the folder we need to select - we don't need it's children
            if (parts.Count <= 1)
            {
                return;
            }

            FtpFolderNode node = folders.FirstOrDefault(n => n.Name == parts[0]);
            if (node != null)
            {
                node.Children = GetFolderList(node.Path);

                FetchNecessaryDecendants(node.Children, descendantPath.Remove(0, node.Name.Length + 1));
            }
        }

        /// <summary>
        /// A row is about to be expanded
        /// </summary>
        private void OnBeforeExpand(object sender, GridRowExpandCollapseEventArgs e)
        {
            if (e.Row.ContentsUnknown)
            {
                e.Row.ContentsUnknown = false;

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    FtpFolderNode node = (FtpFolderNode) e.Row.Tag;
                    CreateGridRows(GetFolderList(node.Path), e.Row.NestedRows);
                }
                catch (NetworkSessionException ex)
                {
                    MessageHelper.ShowError(this, "ShipWorks could not retrieve the list of child folders:\n\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Create rows for each node in the collection, using the given grid parent
        /// </summary>
        private void CreateGridRows(List<FtpFolderNode> nodes, GridRowCollection parentRows)
        {
            foreach (FtpFolderNode node in nodes)
            {
                GridRow row = new GridRow(node.Name, Resources.folderclosed);
                row.ContentsUnknown = node.Children == null;
                row.Tag = node;

                parentRows.Add(row);

                if (node.Children != null)
                {
                    CreateGridRows(node.Children, row.NestedRows);
                }
            }
        }

        /// <summary>
        /// Get the root level folder list
        /// </summary>
        private List<FtpFolderNode> GetFolderList(string path)
        {
            List<FtpFolderNode> folders = new List<FtpFolderNode>();
            
            // Change to the desired path
            ftp.ChangeDirectory(path);

            // Enumerate through each directory
            foreach (FileSystemItem item in ftp.GetList())
            {
                if (item.IsDirectory)
                {
                    string itemPath = (path == "/") ? "/" + item.Name : string.Format("{0}/{1}", path, item.Name);

                    folders.Add(new FtpFolderNode { Name = item.Name, Path = itemPath });
                }
            }

            // Order by name
            folders.Sort(new Comparison<FtpFolderNode>((left, right) => string.Compare(left.Name, right.Name)));

            return folders;
        }
    }
}
