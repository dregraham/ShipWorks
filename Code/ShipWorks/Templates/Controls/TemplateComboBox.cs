using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.UI.Controls;
using System.Windows.Forms;
using System.Drawing;
using ShipWorks.Data.Model.EntityClasses;
using Divelements.SandGrid.Rendering;
using ShipWorks.Filters;
using ShipWorks.Users;
using System.Drawing.Imaging;
using System.ComponentModel;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Properties;
using ShipWorks.Data;
using System.Linq;
using ShipWorks.ApplicationCore.Appearance;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Custom combo box for displaying templates
    /// </summary>
    public class TemplateComboBox : PopupComboBox
    {
        PopupController popupController;

        TemplateTreeControl templateTree;

        // The selected templateID
        long? selectedID;

        TextFormattingInformation textFormat;

        bool sizeToContent = false;

        public event EventHandler SelectedTemplateChanged;

        // The version that the TemplateManager was at when the ComboBox was last loaded
        long lastSyncVersion = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateComboBox()
        {
            textFormat = new TextFormattingInformation();
            textFormat.StringFormat = new StringFormat(StringFormatFlags.NoWrap);
            textFormat.TextFormatFlags = TextFormatFlags.Top;

            // Create the template tree that will be popped up
            templateTree = new TemplateTreeControl();
            templateTree.BorderStyle = BorderStyle.None;
            templateTree.HotTracking = true;
            templateTree.ShowManageTemplates = true;
            
            // When it becomes visible we want to move the selected item into view
            templateTree.VisibleChanged += new EventHandler(OnTemplateTreeVisibleChanged);

            // Create the drop-down
            popupController = new PopupController(templateTree);
            popupController.Animate = PopupAnimation.System;

            // The filter is what we are going to be dropping down
            this.PopupController = popupController;
        }

        /// <summary>
        /// Controls if the snippets folder is displayed
        /// </summary>
        [DefaultValue(TemplateTreeSnippetDisplayType.Hidden)]
        [Category("Behavior")]
        public TemplateTreeSnippetDisplayType SnippetDisplay
        {
            get { return templateTree.SnippetDisplay; }
            set { templateTree.SnippetDisplay = value; }
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        public bool ShowManageTemplates
        {
            get
            {
                return templateTree.ShowManageTemplates;
            }
            set
            {
                templateTree.ShowManageTemplates = value;
            }
        }

        /// <summary>
        /// Load the tree that will display the templates
        /// </summary>
        public void LoadTemplates()
        {
            lastSyncVersion = TemplateManager.Tree.TreeVersion;

            templateTree.SelectedTemplateNodeChanged -= OnTemplateSelected;
            templateTree.LoadTemplates();
            templateTree.SelectedTemplateNodeChanged += OnTemplateSelected;

            templateTree.ApplyFolderState(new FolderExpansionState(UserSession.User.Settings.TemplateExpandedFolders));
        }

        /// <summary>
        /// The currently selected template node.  If TemplateID is an ID of a deleted template, this will be null.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TemplateEntity SelectedTemplate
        {
            get
            {
                if (!SelectedTemplateID.HasValue)
                {
                    return null;
                }

                return TemplateManager.Tree.GetTemplate(SelectedTemplateID.Value);
            }
            set
            {
                SelectedTemplateID = (value != null) ? value.TemplateID : (long?) null;
            }
        }

        /// <summary>
        /// The currently selected template ID.  Can be the ID of a deleted template.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long? SelectedTemplateID
        {
            get
            {
                return selectedID;
            }
            set
            {
                if (value == 0)
                {
                    value = null;
                }

                if (selectedID == value)
                {
                    return;
                }

                selectedID = value;

                UpdateSize();
                Invalidate();

                if (SelectedTemplateChanged != null)
                {
                    SelectedTemplateChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Select the first template in the tree
        /// </summary>
        public void SelectFirstTemplate()
        {
            TemplateEntity template = null;

            foreach (TemplateFolderEntity folder in TemplateManager.Tree.RootFolders)
            {
                template = folder.Templates.FirstOrDefault();

                if (template != null)
                {
                    break;
                }
            }

            SelectedTemplate = template;
        }

        /// <summary>
        /// If true, the ComboBox will auto-size itself to fit whatever text is displayed in its text area.
        /// </summary>
        [DefaultValue(false)]
        public bool SizeToContent
        {
            get
            {
                return sizeToContent;
            }
            set
            {
                sizeToContent = value;

                UpdateSize();
            }
        }

        /// <summary>
        /// Update the size based on the Size To Content setting
        /// </summary>
        private void UpdateSize()
        {
            if (sizeToContent)
            {
                string text = null;
                bool hasImage = false;

                if (SelectedTemplate != null)
                {
                    text = SelectedTemplate.Name;
                    hasImage = true;
                }
                else if (SelectedTemplateID.HasValue)
                {
                    ObjectLabel label = ObjectLabelManager.GetLabel(SelectedTemplateID.Value, true);
                    if (label != null)
                    {
                        text = label.ShortText;
                    }
                }

                if (text != null)
                {
                    using (Graphics g = CreateGraphics())
                    {
                        SizeF size = g.MeasureString(text, Font);
                        Width = Math.Max(100, (int) size.Width + SystemInformation.VerticalScrollBarWidth + (hasImage ? 40 : 24));
                    }
                }
                else
                {
                    Width = 140;
                }
            }
        }

        /// <summary>
        /// Get the ideal size of the popup
        /// </summary>
        protected override Size GetIdealPopupSize()
        {
            TemplateEntity selectedTemplate = SelectedTemplate;

            if (selectedTemplate != null)
            {
                templateTree.EnsureNodeVisible(new TemplateTreeNode(selectedTemplate));
            }

            return templateTree.IdealSize;
        }

        /// <summary>
        /// When a template is selected, we close the drop down
        /// </summary>
        private void OnTemplateSelected(object sender, TemplateNodeChangedEventArgs e)
        {
            SelectedTemplate = e.NewNode.Template;

            // And we can close the drop down
            popupController.Close(DialogResult.OK);
        }

        /// <summary>
        /// The visibility of the tree has changed
        /// </summary>
        void OnTemplateTreeVisibleChanged(object sender, EventArgs e)
        {
            if (templateTree.Visible)
            {
                TemplateEntity selectedTemplate = SelectedTemplate;

                if (lastSyncVersion != TemplateManager.Tree.TreeVersion)
                {
                    LoadTemplates();
                }

                if (selectedTemplate != null)
                {
                    templateTree.EnsureNodeVisible(new TemplateTreeNode(selectedTemplate));
                }

                templateTree.SelectedTemplateNodeChanged -= OnTemplateSelected;
                templateTree.SelectedTemplateTreeNode = null;
                templateTree.SelectedTemplateNodeChanged += OnTemplateSelected;

                if (selectedTemplate != null)
                {
                    templateTree.HotTrackNode = new TemplateTreeNode(selectedTemplate);
                }
                else
                {
                    templateTree.HotTrackNode = null;
                }
            }
            else
            {
                // Save the folder state (could have crashed with it open - check to be sure)
                if (UserSession.IsLoggedOn)
                {
                    UserSession.User.Settings.TemplateExpandedFolders = templateTree.GetFolderState().GetState();
                }
            }
        }

        /// <summary>
        /// Draw the selected filter
        /// </summary>
        protected override void OnDrawSelectedItem(Graphics g, Color foreColor, Rectangle bounds)
        {
            TemplateEntity selectedTemplate = SelectedTemplate;

            Image image = null;
            string text = null;

            if (selectedTemplate != null)
            {
                image = TemplateHelper.GetTemplateImage(selectedTemplate);
                text = selectedTemplate.Name;
            }
            else if (SelectedTemplateID.HasValue)
            {
                ObjectLabel label = ObjectLabelManager.GetLabel(SelectedTemplateID.Value, true);

                if (label != null)
                {
                    image = Resources.delete2_16;
                    text = label.ShortText;
                }
            }

            if (text != null)
            {
                bounds.Offset(1, 0);

                if (Enabled)
                {
                    g.DrawImage(image, bounds.Left, bounds.Top, image.Width, image.Height);
                }
                else
                {
                    ControlPaint.DrawImageDisabled(g, image, bounds.Left, bounds.Top, BackColor);
                }

                int imageTextSeparation = 5;
                bounds.Offset(image.Width + imageTextSeparation, 1);
                bounds.Width -= (image.Width + imageTextSeparation);

                IndependentText.DrawText(g, text, Font, bounds, textFormat, foreColor);
            }
        }
    }
}
