namespace ShipWorks.Templates.Management
{
    partial class TemplateEditorDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelFilterName = new System.Windows.Forms.Label();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.settingsControl = new ShipWorks.Templates.Controls.TemplateSettingsControl();
            this.tabPagePreview = new System.Windows.Forms.TabPage();
            this.previewControl = new ShipWorks.Templates.Controls.TemplatePreviewControl();
            this.tabPageCode = new System.Windows.Forms.TabPage();
            this.xslEditor = new ShipWorks.Templates.Controls.XslEditing.TemplateXslEditorControl();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.editSnippet = new ShipWorks.UI.Controls.DropDownButton();
            this.contextMenuSnippets = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.templateName = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.tabPageSettings.SuspendLayout();
            this.tabPagePreview.SuspendLayout();
            this.tabPageCode.SuspendLayout();
            this.tabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(514, 557);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "Save";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(595, 557);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelFilterName
            // 
            this.labelFilterName.Location = new System.Drawing.Point(2, 15);
            this.labelFilterName.Name = "labelFilterName";
            this.labelFilterName.Size = new System.Drawing.Size(49, 13);
            this.labelFilterName.TabIndex = 2;
            this.labelFilterName.Text = "Name:";
            this.labelFilterName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.settingsControl);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(650, 481);
            this.tabPageSettings.TabIndex = 3;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // settingsControl
            // 
            this.settingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.settingsControl.Location = new System.Drawing.Point(3, 3);
            this.settingsControl.Name = "settingsControl";
            this.settingsControl.Size = new System.Drawing.Size(644, 475);
            this.settingsControl.TabIndex = 0;
            // 
            // tabPagePreview
            // 
            this.tabPagePreview.Controls.Add(this.previewControl);
            this.tabPagePreview.Location = new System.Drawing.Point(4, 22);
            this.tabPagePreview.Name = "tabPagePreview";
            this.tabPagePreview.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePreview.Size = new System.Drawing.Size(650, 481);
            this.tabPagePreview.TabIndex = 2;
            this.tabPagePreview.Text = "Preview";
            this.tabPagePreview.UseVisualStyleBackColor = true;
            // 
            // previewControl
            // 
            this.previewControl.BackColor = System.Drawing.SystemColors.Control;
            this.previewControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.previewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.previewControl.Location = new System.Drawing.Point(3, 3);
            this.previewControl.Name = "previewControl";
            this.previewControl.Size = new System.Drawing.Size(644, 475);
            this.previewControl.TabIndex = 0;
            // 
            // tabPageCode
            // 
            this.tabPageCode.Controls.Add(this.xslEditor);
            this.tabPageCode.Location = new System.Drawing.Point(4, 22);
            this.tabPageCode.Name = "tabPageCode";
            this.tabPageCode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCode.Size = new System.Drawing.Size(650, 481);
            this.tabPageCode.TabIndex = 1;
            this.tabPageCode.Text = "Code";
            this.tabPageCode.UseVisualStyleBackColor = true;
            // 
            // xslEditor
            // 
            this.xslEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xslEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.xslEditor.FooterXsl = null;
            this.xslEditor.HeaderXsl = null;
            this.xslEditor.Location = new System.Drawing.Point(3, 3);
            this.xslEditor.Name = "xslEditor";
            this.xslEditor.Size = new System.Drawing.Size(644, 475);
            this.xslEditor.TabIndex = 0;
            this.xslEditor.TemplateXsl = "";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageCode);
            this.tabControl.Controls.Add(this.tabPagePreview);
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Location = new System.Drawing.Point(12, 44);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(658, 507);
            this.tabControl.TabIndex = 6;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.OnTabSelected);
            this.tabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.OnTabDeselecting);
            // 
            // editSnippet
            // 
            this.editSnippet.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editSnippet.AutoSize = true;
            this.editSnippet.ContextMenuStrip = this.contextMenuSnippets;
            this.editSnippet.Image = global::ShipWorks.Properties.Resources.template_snippet16;
            this.editSnippet.Location = new System.Drawing.Point(550, 10);
            this.editSnippet.Name = "editSnippet";
            this.editSnippet.Size = new System.Drawing.Size(120, 23);
            this.editSnippet.SplitButton = false;
            this.editSnippet.SplitContextMenu = this.contextMenuSnippets;
            this.editSnippet.TabIndex = 7;
            this.editSnippet.Text = "Edit Snippet";
            this.editSnippet.UseVisualStyleBackColor = true;
            this.editSnippet.Click += new System.EventHandler(this.OnEditSnippet);
            this.editSnippet.DropDownShowing += new System.EventHandler(this.OnSnippetMenuShowing);
            // 
            // contextMenuSnippets
            // 
            this.contextMenuSnippets.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuSnippets.Name = "snippetMenu";
            this.contextMenuSnippets.Size = new System.Drawing.Size(61, 4);
            // 
            // templateName
            // 
            this.templateName.Location = new System.Drawing.Point(57, 12);
            this.fieldLengthProvider.SetMaxLengthSource(this.templateName, ShipWorks.Data.Utility.EntityFieldLengthSource.TemplateName);
            this.templateName.Name = "templateName";
            this.templateName.Size = new System.Drawing.Size(252, 21);
            this.templateName.TabIndex = 3;
            // 
            // TemplateEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(680, 592);
            this.Controls.Add(this.editSnippet);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.templateName);
            this.Controls.Add(this.labelFilterName);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TemplateEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Template Editor";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Activated += new System.EventHandler(this.OnActivated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.tabPageSettings.ResumeLayout(false);
            this.tabPagePreview.ResumeLayout(false);
            this.tabPageCode.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TextBox templateName;
        private System.Windows.Forms.Label labelFilterName;
        private System.Windows.Forms.TabPage tabPageSettings;
        private ShipWorks.Templates.Controls.TemplateSettingsControl settingsControl;
        private System.Windows.Forms.TabPage tabPagePreview;
        private ShipWorks.Templates.Controls.TemplatePreviewControl previewControl;
        private System.Windows.Forms.TabPage tabPageCode;
        private ShipWorks.Templates.Controls.XslEditing.TemplateXslEditorControl xslEditor;
        private System.Windows.Forms.TabControl tabControl;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private ShipWorks.UI.Controls.DropDownButton editSnippet;
        private System.Windows.Forms.ContextMenuStrip contextMenuSnippets;
    }
}