using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using ShipWorks.UI.Controls;
using ShipWorks.Properties;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Templates.Controls;
using ShipWorks.Templates.Processing;
using ShipWorks.Filters;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;
using System.Diagnostics;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore.MessageBoxes;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Window for editing templates
    /// </summary>
    public partial class TemplateEditorDlg : Form
    {
        TemplateEntity template;

        SizeGripRenderer sizeGripRenderer;

        List<SnippetEditorDlg> snippetEditors = new List<SnippetEditorDlg>();

        // Remember the bounds (for this session of editing) on a per-template basis, so if your constantly closing\opening, it stays where you had it.
        Dictionary<TemplateEntity, Rectangle> snippetEditorBounds = new Dictionary<TemplateEntity, Rectangle>();

        // The last TemplateXsl version that we previewed.  If it hasn't changed, we'll know we don't need to update the preview window.
        Guid lastPreviewXslVersion = Guid.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateEditorDlg(TemplateEntity template)
        {
            InitializeComponent();

            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            if (template.IsReadOnly)
            {
                throw new InvalidOperationException("Cannot pass a ReadOnly Template to the template editor.");
            }

            if (template.TemplateTree as TemplateTree == null)
            {
                throw new InvalidOperationException("Cannot pass a Template not attached to a TemplateTree to the template editor.");
            }

            WindowStateSaver.Manage(this);

            this.template = template;

            previewControl.Initialize();

            sizeGripRenderer = new SizeGripRenderer(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageTemplates);

            templateName.Text = template.Name;

            // Snippets don't have their own settings or preview output
            if (template.IsSnippet)
            {
                tabControl.TabPages.Remove(tabPagePreview);
                tabControl.TabPages.Remove(tabPageSettings);
            }

            LoadTabSettings(tabControl.SelectedTab);
        }

        /// <summary>
        /// Called when the Form is activated.  Primarily we care about when we are coming back from a snippet editor having focus, but there's no
        /// way to detect that specifically so we use this general purpose.
        /// </summary>
        private void OnActivated(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPagePreview)
            {
                if (lastPreviewXslVersion != TemplateXslProvider.FromTemplate(template).Version)
                {
                    // If we didn't do the POST then the input would be consumed by the preview - so like if the Activate was due to clicking a button, but button click would 
                    // get ignored.
                    BeginInvoke(new MethodInvoker(() =>
                        {
                            previewControl.SaveSettingsToTemplate();
                            UpdatePreview();
                        }));
                }
            }
        }

        /// <summary>
        /// User is moving off a tab
        /// </summary>
        private void OnTabDeselecting(object sender, TabControlCancelEventArgs e)
        {
            if (!SaveTabSettings(e.TabPage))
            {
                e.Cancel = true;
                return;
            }

            // If we are moving off of the code editor, ensure the find\replace window is closed
            if (e.TabPage == tabPageCode)
            {
                xslEditor.CloseFindReplace();
            }
        }

        /// <summary>
        /// User has moved on to a tab
        /// </summary>
        private void OnTabSelected(object sender, TabControlEventArgs e)
        {
            LoadTabSettings(e.TabPage);
        }

        /// <summary>
        /// Load the settings for the specified tab that was just selected
        /// </summary>
        private void LoadTabSettings(TabPage tabPage)
        {
            if (tabPage == tabPageCode)
            {
                // Don't reload the XSL if its the same... (no settings have changed).  Otherwise undo\redo and the cursor
                // position would be lost.
                if (xslEditor.TemplateXsl != template.Xsl)
                {
                    xslEditor.TemplateXsl = template.Xsl;
                }

                ActiveControl = xslEditor;
            }

            if (tabPage == tabPagePreview)
            {
                UpdatePreview();
            }

            if (tabPage == tabPageSettings)
            {
                settingsControl.LoadSettings(template);
            }
        }

        /// <summary>
        /// Update the preview
        /// </summary>
        private void UpdatePreview()
        {
            if (lastPreviewXslVersion != TemplateXslProvider.FromTemplate(template).Version)
            {
                lastPreviewXslVersion = TemplateXslProvider.FromTemplate(template).Version;
                previewControl.LoadPreview(template, true);
            }
        }

        /// <summary>
        /// Save the settings of the specified tab
        /// </summary>
        private bool SaveTabSettings(TabPage tabPage)
        {
            if (tabPage == tabPageCode)
            {
                template.Xsl = xslEditor.TemplateXsl;

                // If the XSL is valid, update the settings
                if (TemplateXslProvider.FromTemplate(template).IsValid)
                {
                    TemplateSettingsSynchronizer.UpdateSettingsFromXsl(template);
                }
            }

            if (tabPage == tabPagePreview)
            {
                previewControl.SaveSettingsToTemplate();
            }

            if (tabPage == tabPageSettings)
            {
                if (!settingsControl.SaveSettingsToTemplate())
                {
                    return false;
                }

                // If the XSL is valid, update it
                if (TemplateXslProvider.FromTemplate(template).IsValid)
                {
                    TemplateSettingsSynchronizer.UpdateXslFromSettings(template);
                }
            }

            return true;
        }
        
        /// <summary>
        /// Rollback all changes we have made since the editor opened.
        /// </summary>
        private void RollbackChanges()
        {
            template.RollbackChanges();

            // Rollback all the snippet changes
            foreach (SnippetEditorDlg snippetDlg in snippetEditors.ToList())
            {
                snippetDlg.CancelAndClose();
            }
        }

        /// <summary>
        /// The snippet context menu is about to be shown
        /// </summary>
        private void OnSnippetMenuShowing(object sender, EventArgs e)
        {
            contextMenuSnippets.Items.Clear();

            // Use the known editable tree of the template we are editing
            TemplateTree templateTree = (TemplateTree) template.TemplateTree;

            TemplateFolderEntity snippetFolder = templateTree.GetFolder(TemplateBuiltinFolders.SnippetsFolderID);
            if (snippetFolder != null)
            {
                foreach (TemplateFolderEntity folder in snippetFolder.ChildFolders)
                {
                    ToolStripMenuItem menuFolder = new ToolStripMenuItem(folder.Name, Resources.folderclosed);
                    contextMenuSnippets.Items.Add(menuFolder);

                    LoadSnippetFolder(menuFolder.DropDownItems, folder);
                }

                LoadSnippetFolder(contextMenuSnippets.Items, snippetFolder);
            }

            if (contextMenuSnippets.Items.Count == 0)
            {
                contextMenuSnippets.Items.Add("(No snippets)").Enabled = false;
            }
        }

        /// <summary>
        /// Load the snippets of the given folder into the given ToolStripItemCollection
        /// </summary>
        private void LoadSnippetFolder(ToolStripItemCollection itemCollection, TemplateFolderEntity folder)
        {
            foreach (TemplateEntity template in folder.Templates)
            {
                ToolStripItem menuItem = itemCollection.Add(template.Name, TemplateHelper.GetTemplateImage(template));
                menuItem.Tag = template;
                menuItem.Click += new EventHandler(OnEditSnippet);

                if (template.FullName == this.template.FullName)
                {
                    menuItem.Enabled = false;
                    menuItem.Text += " (Editing)";
                }

                if (snippetEditors.Any(f => f.Template == template))
                {
                    menuItem.Text += " (Open)";
                }
            }

            if (itemCollection.Count == 0)
            {
                itemCollection.Add("(No snippets)").Enabled = false;
            }
        }

        /// <summary>
        /// Open the snippet editor for a snippet chosen by the dropdown menu
        /// </summary>
        private void OnEditSnippet(object sender, EventArgs e)
        {
            ToolStripItem menuItem = (ToolStripItem) sender;
            TemplateEntity template = (TemplateEntity) menuItem.Tag;

            // If it is already open just activate it
            SnippetEditorDlg existingDlg = snippetEditors.FirstOrDefault(f => f.Template == template);
            if (existingDlg != null)
            {
                existingDlg.Activate();
            }
            else
            {
                SnippetEditorDlg editorDlg = new SnippetEditorDlg(template);
                
                // If we already know the bounds of the editor used for this template (for this session) then use it, so if you keep closing\opening
                // the same templates, the bounds are remembered.
                if (snippetEditorBounds.ContainsKey(template))
                {
                    editorDlg.Bounds = snippetEditorBounds[template];
                }
                else
                {
                    // If this is the first one, just center it
                    if (snippetEditors.Count == 0)
                    {
                        CenterSnippetEditor(editorDlg);
                    }
                    else
                    {
                        SnippetEditorDlg lastActiveEditor = snippetEditors.Last();


                        Point desiredLocation = lastActiveEditor.Location;
                        desiredLocation.Offset(25, 25);

                        editorDlg.Location = desiredLocation;
                        editorDlg.Size = lastActiveEditor.Size;
                    }
                }

                snippetEditors.Add(editorDlg);

                editorDlg.Activated += new EventHandler(OnSnippetEditorActivated);
                editorDlg.FormClosed += new FormClosedEventHandler(OnSnippetEditorClosed);

                editorDlg.Show(this);
            }
        }

        /// <summary>
        /// Whenever the snippet editor is activated, move it to the back of our list.  This helps us to keep kind of a z-order.
        /// </summary>
        void OnSnippetEditorActivated(object sender, EventArgs e)
        {
            SnippetEditorDlg editor = (SnippetEditorDlg) sender;
            snippetEditors.Remove(editor);
            snippetEditors.Add(editor);
        }

        /// <summary>
        /// A snippet editor window has closed
        /// </summary>
        void OnSnippetEditorClosed(object sender, FormClosedEventArgs e)
        {
            SnippetEditorDlg dlg = (SnippetEditorDlg) sender;

            snippetEditorBounds[dlg.Template] = dlg.Bounds;
            snippetEditors.Remove(dlg);

            // Closing the toolwindow may not activate our window, so make sure the preview gets updated
            if (tabControl.SelectedTab == tabPagePreview)
            {
                UpdatePreview();
            }
        }

        /// <summary>
        /// Center the snippet edtior on this window
        /// </summary>
        private void CenterSnippetEditor(SnippetEditorDlg editorDlg)
        {
            // Find our center point
            Point center = new Point(Left + (Width / 2), Top + (Height / 2));

            // Align the center from the editor
            editorDlg.Left = center.X - (editorDlg.Width / 2);
            editorDlg.Top = center.Y - (editorDlg.Height / 2);
        }

        /// <summary>
        /// Save changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            string proposedName = templateName.Text.Trim();
            if (proposedName.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter a name for the template.");
                ActiveControl = templateName;
                return;
            }

            template.Name = proposedName;

            if (!SaveTabSettings(tabControl.SelectedTab))
            {
                return;
            }

            try
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Save all the snippets
                    foreach (SnippetEditorDlg snippetDlg in snippetEditors.ToList())
                    {
                        // If it doesn't work, it would have displayed an error
                        if (!snippetDlg.SaveAndClose())
                        {
                            return;
                        }
                    }

                    // Save this template
                    TemplateEditingService.SaveTemplate(template, true, adapter);

                    adapter.Commit();
                }

                DialogResult = DialogResult.OK;
            }
            catch (TemplateConcurrencyException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                RollbackChanges();

                DialogResult = DialogResult.Abort;
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                if (!SaveTabSettings(tabControl.SelectedTab))
                {
                    e.Cancel = true;
                    return;
                }

                bool snippetsDirty = snippetEditors.Any(f => f.IsDirty);

                if (template.IsDirty || snippetsDirty)
                {
                    using (UnsavedChangesDlg dlg = new UnsavedChangesDlg())
                    {
                        if (snippetsDirty)
                        {
                            if (template.IsDirty)
                            {
                                dlg.Message = "The template and snippets have unsaved changes.";
                            }
                            else
                            {
                                dlg.Message = "Some open snippets have unsaved changes.";
                            }
                        }
                        else
                        {
                            dlg.Message = "The template has unsaved changes.";
                        }

                        DialogResult result = dlg.ShowDialog(this);

                        if (result == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            e.Cancel = true;
                            BeginInvoke(new Action<object, EventArgs>(OnOK), null, EventArgs.Empty);
                            return;
                        }
                    }
                }

                RollbackChanges();
            }
        }
    }
}