using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Controls;
using ShipWorks.UI;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Window for adding a new template folder
    /// </summary>
    public partial class AddTemplateFolderDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddTemplateFolderDlg(string initialName, FolderExpansionState initialState)
        {
            InitializeComponent();

            name.Text = initialName;

            templateTree.LoadTemplates();
            templateTree.ApplyFolderState(initialState);

            templateTree.SelectedTemplateTreeNode = TemplateTreeNode.RootNode;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageTemplates);

            name.SelectAll();
        }

        /// <summary>
        /// The name chosen for the folder
        /// </summary>
        public string FolderName
        {
            get { return name.Text.Trim(); }
        }

        /// <summary>
        /// The folder that was selected by the user
        /// </summary>
        public TemplateFolderEntity SelectedFolder
        {
            get
            {
                TemplateTreeNode treeNode = templateTree.SelectedTemplateTreeNode;
                if (treeNode == null)
                {
                    return null;
                }

                if (treeNode.IsRoot)
                {
                    return null;
                }

                return treeNode.Folder;
            }
            set
            {
                if (value == null)
                {
                    templateTree.SelectedTemplateTreeNode = TemplateTreeNode.RootNode;
                }
                else
                {
                    templateTree.SelectedTemplateTreeNode = new TemplateTreeNode(value);
                }
            }
        }

        /// <summary>
        /// Validate selection before closing
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (FolderName.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter a name for the folder.");
                ActiveControl = name;
                return;
            }

            if (SelectedFolder == null && FolderName == "System")
            {
                MessageHelper.ShowError(this, "The folder name 'System' is reserved and cannot be used.");
                ActiveControl = name;
                return;
            }

            if (SelectedFolder != null && SelectedFolder.IsSystemFolder && FolderName == "Snippets")
            {
                MessageHelper.ShowError(this, @"The folder name 'System\Snippets' is reserved and cannot be used.");
                ActiveControl = name;
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}