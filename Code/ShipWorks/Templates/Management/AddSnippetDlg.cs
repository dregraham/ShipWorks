using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Templates.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;
using ShipWorks.Templates.Media;
using ShipWorks.Templates.Saving;
using ShipWorks.Templates.Management.Skeletons;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Window for adding a new snippet to ShipWorks
    /// </summary>
    public partial class AddSnippetDlg : Form
    {
        TemplateTree templateTree;
        TemplateEntity template = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddSnippetDlg(TemplateFolderEntity initialFolder, FolderExpansionState initialState)
        {
            InitializeComponent();

            if (initialFolder == null)
            {
                throw new ArgumentNullException("initialFolder");
            }

            templateTree = initialFolder.TemplateTree as TemplateTree;
            if (templateTree == null)
            {
                throw new InvalidOperationException("The given initial folder must belong to a TemplateTree.");
            }

            // Dont show choosing for a folder if there is only the single snippets folder
            TemplateFolderEntity snippetsFolder = templateTree.GetFolder(TemplateBuiltinFolders.SnippetsFolderID);
            if (snippetsFolder.ChildFolders.Count == 0)
            {
                panelFolders.Visible = false;
                Height -= panelFolders.Height;
            }
            else
            {
                treeControl.LoadTemplates(templateTree);
                treeControl.ApplyFolderState(initialState);

                treeControl.SelectedTemplateTreeNode = new TemplateTreeNode(initialFolder);
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageTemplates);
        }

        /// <summary>
        /// The Template that was created, if ShowDialog returns DialogResult.OK
        /// </summary>
        public TemplateEntity Template
        {
            get
            {
                if (DialogResult != DialogResult.OK)
                {
                    return null;
                }

                return template;
            }
        }
        
        /// <summary>
        /// Create the snippet
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (name.Text.Trim().Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter a name for the snippet.");
                return;
            }

            template = new TemplateEntity();
            template.Name = name.Text.Trim();

            // Parent
            template.ParentFolder = (treeControl.SelectedTemplateTreeNode != null) ? treeControl.SelectedTemplateTreeNode.Folder : templateTree.GetFolder(TemplateBuiltinFolders.SnippetsFolderID);
            template.TemplateTree = template.ParentFolder?.TemplateTree;

            // The initial XSL
            template.Xsl = TemplateSkeletons.GetSnippetSkeleton(template.Name);

            // The other values don't matter
            template.InitializeNullsToDefault();

            try
            {
                TemplateEditingService.SaveTemplate(template);

                DialogResult = DialogResult.OK;
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                DialogResult = DialogResult.Abort;
            }
        }
    }
}
