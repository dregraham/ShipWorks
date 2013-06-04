namespace ShipWorks.Templates.Management
{
    partial class AddSnippetDlg
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
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.labelLocation = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.panelFolders = new System.Windows.Forms.Panel();
            this.treeControl = new ShipWorks.Templates.Controls.TemplateTreeControl();
            this.panelFolders.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(215, 298);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(134, 298);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 3;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(-2, 3);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(213, 13);
            this.labelLocation.TabIndex = 0;
            this.labelLocation.Text = "Select the folder to put the new snippet in:";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(12, 24);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(277, 21);
            this.name.TabIndex = 1;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(9, 8);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(99, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "New snippet name:";
            // 
            // panelFolders
            // 
            this.panelFolders.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFolders.Controls.Add(this.labelLocation);
            this.panelFolders.Controls.Add(this.treeControl);
            this.panelFolders.Location = new System.Drawing.Point(12, 51);
            this.panelFolders.Name = "panelFolders";
            this.panelFolders.Size = new System.Drawing.Size(277, 240);
            this.panelFolders.TabIndex = 2;
            // 
            // templateTree
            // 
            this.treeControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeControl.FoldersOnly = true;
            this.treeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.treeControl.HotTrackNode = null;
            this.treeControl.Location = new System.Drawing.Point(1, 19);
            this.treeControl.Name = "templateTree";
            this.treeControl.Size = new System.Drawing.Size(276, 221);
            this.treeControl.SnippetDisplay = ShipWorks.Templates.Controls.TemplateTreeSnippetDisplayType.OnlySnippets;
            this.treeControl.TabIndex = 1;
            // 
            // AddSnippetDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(302, 333);
            this.Controls.Add(this.panelFolders);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.name);
            this.Controls.Add(this.labelName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddSnippetDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Snippet";
            this.Load += new System.EventHandler(this.OnLoad);
            this.panelFolders.ResumeLayout(false);
            this.panelFolders.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private ShipWorks.Templates.Controls.TemplateTreeControl treeControl;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Panel panelFolders;
    }
}