using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared;
using ShipWorks.Filters.Management;
using ShipWorks.Filters.Controls;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Templates.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.ApplicationCore.Appearance;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Window for allowing a user to select where a template is going to go
    /// </summary>
    public partial class ChooseTemplateLocationDlg : Form
    {
        // Event raised when the user is saving their selection
        public event CancelEventHandler Saving;

        // The template or folder that's having its location chosen
        TemplateTreeNode subject;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChooseTemplateLocationDlg(string action, TemplateTreeNode subject, FolderExpansionState initialState)
        {
            InitializeComponent();

            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }

            if (initialState == null)
            {
                throw new ArgumentNullException("initialState");
            }

            this.subject = subject;

            if (subject.IsFolder && !subject.IsSnippetNode)
            {
                templateTree.ShowFoldersRoot = true;
            }

            labelInstructions.Text = string.Format(labelInstructions.Text, action, subject.IsFolder ? "folder" : (subject.IsSnippetNode ? "snippet" : "template"));
            nodeIcon.Image = subject.Image;
            labelNodeName.Text = subject.Name;

            this.Text = string.Format("{0} {1}", action, subject.IsFolder ? "Folder" : (subject.IsSnippetNode ? "Snippet" : "Template"));

            templateTree.SnippetDisplay = subject.IsSnippetNode ? TemplateTreeSnippetDisplayType.OnlySnippets : TemplateTreeSnippetDisplayType.Hidden;
            templateTree.LoadTemplates();
            templateTree.ApplyFolderState(initialState);


            UserSession.Security.DemandPermission(PermissionType.ManageTemplates);
        }

        /// <summary>
        /// The node that is the reason this window is open
        /// </summary>
        public TemplateTreeNode SubjectNode
        {
            get
            {
                return subject;
            }
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
        /// User has ok'd the close of the window
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (Saving != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                Saving(this, args);

                // Cancel the closing of the window
                if (args.Cancel)
                {
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}