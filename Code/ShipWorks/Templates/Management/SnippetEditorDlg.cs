using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.MessageBoxes;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Window for editing snippets.  Used as a child modeless window of the Template Editor
    /// </summary>
    public partial class SnippetEditorDlg : Form
    {
        TemplateEntity template;
        SizeGripRenderer sizeGripRenderer;

        /// <summary>
        /// Constructor
        /// </summary>
        public SnippetEditorDlg(TemplateEntity template)
        {
            InitializeComponent();

            this.template = template;

            Text += " - " + template.FullName.Replace(@"System\Snippets\", "");

            sizeGripRenderer = new SizeGripRenderer(this);
        }
        
        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            xslEditor.TemplateXsl = template.Xsl;
        }

        /// <summary>
        /// The template being edited
        /// </summary>
        public TemplateEntity Template
        {
            get { return template; }
        }

        /// <summary>
        /// Indicates if the template has changed and needs to be saved.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return template.IsDirty || (template.Xsl != xslEditor.TemplateXsl);
            }
        }

        /// <summary>
        /// As the user leaves the window update the in-memory template XSL so it can show up in previews.
        /// </summary>
        private void OnDeactivate(object sender, EventArgs e)
        {
            template.Xsl = xslEditor.TemplateXsl;
        }

        /// <summary>
        /// Save any changes to the snippet and close the window
        /// </summary>
        public bool SaveAndClose()
        {
            OnSave(save, EventArgs.Empty);

            return DialogResult != DialogResult.None;
        }

        /// <summary>
        /// Cancel any changes to the snippet and close the window
        /// </summary>
        public void CancelAndClose()
        {
            // Make sure the OnFormClosing does not prompt to save
            template.RollbackChanges();
            xslEditor.TemplateXsl = template.Xsl;

            OnCancel(cancel, EventArgs.Empty);
        }

        /// <summary>
        /// Save the template to the database
        /// </summary>
        private void OnSave(object sender, EventArgs e)
        {
            template.Xsl = xslEditor.TemplateXsl;

            try
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    TemplateEditingService.SaveTemplate(template, true, adapter);

                    adapter.Commit();
                }

                CloseEditor(DialogResult.OK);
            }
            catch (TemplateConcurrencyException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                template.RollbackChanges();

                CloseEditor(DialogResult.Abort);
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Close the window with the Cancel flag set
        /// </summary>
        private void OnCancel(object sender, EventArgs e)
        {
            CloseEditor(DialogResult.Cancel);
        }

        /// <summary>
        /// Close the editor with the specified result flag
        /// </summary>
        private void CloseEditor(DialogResult dialogResult)
        {
            DialogResult = dialogResult;
            Close();
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                template.Xsl = xslEditor.TemplateXsl;

                if (template.IsDirty)
                {
                    using (UnsavedChangesDlg dlg = new UnsavedChangesDlg())
                    {
                        dlg.Message = "The snippet has unsaved changes.";

                        DialogResult result = dlg.ShowDialog(this);

                        if (result == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            e.Cancel = true;
                            BeginInvoke(new Action<object, EventArgs>(OnSave), null, EventArgs.Empty);
                            return;
                        }
                    }
                }

                template.RollbackChanges();
            }

            // Unhook Deactivate so we don't re-edit the XSL as the window closes
            Deactivate -= this.OnDeactivate;
        }
    }
}
