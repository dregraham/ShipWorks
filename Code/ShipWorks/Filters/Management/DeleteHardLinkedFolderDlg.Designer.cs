namespace ShipWorks.Filters.Management
{
    partial class DeleteHardLinkedFolderDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteHardLinkedFolderDlg));
            this.cancel = new System.Windows.Forms.Button();
            this.deleteAll = new System.Windows.Forms.Button();
            this.deleteSelected = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelUsages = new System.Windows.Forms.Panel();
            this.labelWarning = new System.Windows.Forms.Label();
            this.warningIcon = new System.Windows.Forms.PictureBox();
            this.usages = new System.Windows.Forms.TextBox();
            this.labelInUse = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.panelUsages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.warningIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(336, 225);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // deleteAll
            // 
            this.deleteAll.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteAll.Location = new System.Drawing.Point(195, 225);
            this.deleteAll.Name = "deleteAll";
            this.deleteAll.Size = new System.Drawing.Size(135, 23);
            this.deleteAll.TabIndex = 8;
            this.deleteAll.Text = "Delete Folder Completely";
            this.deleteAll.UseVisualStyleBackColor = true;
            this.deleteAll.Click += new System.EventHandler(this.OnDeleteAll);
            // 
            // deleteSelected
            // 
            this.deleteSelected.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteSelected.Location = new System.Drawing.Point(53, 225);
            this.deleteSelected.Name = "deleteSelected";
            this.deleteSelected.Size = new System.Drawing.Size(135, 23);
            this.deleteSelected.TabIndex = 6;
            this.deleteSelected.Text = "Delete Selected Link";
            this.deleteSelected.UseVisualStyleBackColor = true;
            this.deleteSelected.Click += new System.EventHandler(this.OnDeleteSelected);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(48, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(359, 33);
            this.label2.TabIndex = 9;
            this.label2.Text = "Do you want to delete only the selected link, or permanently delete the folder in" +
                " each location that it is linked?";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(48, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "The selected folder is linked.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(7, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(48, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(319, 33);
            this.label3.TabIndex = 11;
            this.label3.Text = "Note: If the folder contains filters or folders that are linked, then only the in" +
                "dividual links contained in this folder will be removed.";
            // 
            // panelUsages
            // 
            this.panelUsages.Controls.Add(this.labelWarning);
            this.panelUsages.Controls.Add(this.warningIcon);
            this.panelUsages.Controls.Add(this.usages);
            this.panelUsages.Controls.Add(this.labelInUse);
            this.panelUsages.Location = new System.Drawing.Point(46, 96);
            this.panelUsages.Name = "panelUsages";
            this.panelUsages.Size = new System.Drawing.Size(373, 123);
            this.panelUsages.TabIndex = 12;
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelWarning.Location = new System.Drawing.Point(21, 9);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(54, 13);
            this.labelWarning.TabIndex = 7;
            this.labelWarning.Text = "Warning";
            // 
            // warningIcon
            // 
            this.warningIcon.Image = ((System.Drawing.Image) (resources.GetObject("warningIcon.Image")));
            this.warningIcon.Location = new System.Drawing.Point(5, 8);
            this.warningIcon.Name = "warningIcon";
            this.warningIcon.Size = new System.Drawing.Size(22, 16);
            this.warningIcon.TabIndex = 159;
            this.warningIcon.TabStop = false;
            // 
            // usages
            // 
            this.usages.Location = new System.Drawing.Point(4, 28);
            this.usages.Multiline = true;
            this.usages.Name = "usages";
            this.usages.ReadOnly = true;
            this.usages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.usages.Size = new System.Drawing.Size(360, 88);
            this.usages.TabIndex = 8;
            // 
            // labelInUse
            // 
            this.labelInUse.AutoSize = true;
            this.labelInUse.Location = new System.Drawing.Point(75, 9);
            this.labelInUse.Name = "labelInUse";
            this.labelInUse.Size = new System.Drawing.Size(230, 13);
            this.labelInUse.TabIndex = 4;
            this.labelInUse.Text = "Some of the items being deleted are in use by:";
            // 
            // DeleteHardLinkedFolderDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(423, 260);
            this.Controls.Add(this.panelUsages);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.deleteAll);
            this.Controls.Add(this.deleteSelected);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteHardLinkedFolderDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delete Folder";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.panelUsages.ResumeLayout(false);
            this.panelUsages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.warningIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button deleteAll;
        private System.Windows.Forms.Button deleteSelected;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelUsages;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.PictureBox warningIcon;
        private System.Windows.Forms.TextBox usages;
        private System.Windows.Forms.Label labelInUse;
    }
}