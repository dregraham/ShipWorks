using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Templates.Controls;
using ShipWorks.UI;
using ShipWorks.Data;
using ShipWorks.Filters.Management;
using ShipWorks.Templates.Media;
using ShipWorks.Data.Connection;
using System.IO;
using ShipWorks.Templates.Saving;
using ShipWorks.Users.Security;
using ShipWorks.Users;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;
using ShipWorks.Templates.Management.Skeletons;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Wizard for creating a new template
    /// </summary>
    public partial class AddTemplateWizard : WizardForm
    {
        TemplateTree templateTree;

        // Template being created
        TemplateEntity template;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddTemplateWizard(TemplateFolderEntity initialFolder, FolderExpansionState initialState)
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

            treeControl.LoadTemplates(templateTree);
            treeControl.ApplyFolderState(initialState);

            treeControl.SelectedTemplateTreeNode = new TemplateTreeNode(initialFolder);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageTemplates);

            template = new TemplateEntity();
            template.Type = (int) TemplateType.Standard;
            template.Xsl = "";
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
        /// Get the UI selected template type
        /// </summary>
        private TemplateType SelectedTemplateType
        {
            get
            {
                if (templateTypeStandard.Checked) return TemplateType.Standard;
                if (templateTypeLabels.Checked) return TemplateType.Label;

                return TemplateType.Report;
            }
        }

        /// <summary>
        /// Get the UI selected output format
        /// </summary>
        private TemplateOutputFormat SelectedOutputFormat
        {
            get
            {
                if (templateFormatHtml.Checked) return TemplateOutputFormat.Html;
                if (templateFormatPlainText.Checked) return TemplateOutputFormat.Text;

                return TemplateOutputFormat.Xml;
            }
        }

        /// <summary>
        /// Stepping next from the name and location page
        /// </summary>
        private void OnStepNextNameAndLocation(object sender, WizardStepEventArgs e)
        {
            string templateName = name.Text.Trim();
            if (templateName.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter a name.");
                e.NextPage = CurrentPage;
                return;
            }

            // We have the builtin "virtual" "System\Snippets" template that gets dyamically generated any time
            // the contengs of the Snippets folder changes to import each snippet - we can't let the user
            // confuse things with a template by that same name.
            if (templateName == "Snippets" && treeControl.SelectedTemplateTreeNode.Folder.IsSystemFolder)
            {
                MessageHelper.ShowError(this, TemplateHelper.SnippetTemplateReservedNameError);
                e.NextPage = CurrentPage;
                return;
            }

            template.Name = templateName;
            template.ParentFolder = treeControl.SelectedTemplateTreeNode.Folder;
            template.TemplateTree = template.ParentFolder?.TemplateTree;
        }

        /// <summary>
        /// Clicking Finish
        /// </summary>
        private void OnStepNextLastPage(object sender, WizardStepEventArgs e)
        {
            // The initial XSL
            template.Xsl = TemplateSkeletons.GetTemplateSkeleton(SelectedTemplateType, SelectedOutputFormat);

            // Settings
            TemplateHelper.ApplyDefaultSettings(template, SelectedTemplateType, SelectedOutputFormat);

            try
            {
                TemplateEditingService.SaveTemplate(template);
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                DialogResult = DialogResult.Abort;
            }
        }
    }
}