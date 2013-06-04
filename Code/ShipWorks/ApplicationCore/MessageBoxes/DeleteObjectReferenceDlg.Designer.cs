namespace ShipWorks.ApplicationCore.MessageBoxes
{
    partial class DeleteObjectReferenceDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteObjectReferenceDlg));
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelInUse = new System.Windows.Forms.Label();
            this.labelWarning = new System.Windows.Forms.Label();
            this.usages = new System.Windows.Forms.TextBox();
            this.panelUsages = new System.Windows.Forms.Panel();
            this.warningIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.panelUsages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.warningIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(223, 178);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "Delete";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(304, 178);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelMessage
            // 
            this.labelMessage.Location = new System.Drawing.Point(50, 10);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(333, 32);
            this.labelMessage.TabIndex = 2;
            this.labelMessage.Text = "Delete folder \'System\\Whatever\\Cool\' and all of its contents?";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image) (resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(32, 32);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            // 
            // labelInUse
            // 
            this.labelInUse.AutoSize = true;
            this.labelInUse.Location = new System.Drawing.Point(75, 9);
            this.labelInUse.Name = "labelInUse";
            this.labelInUse.Size = new System.Drawing.Size(230, 13);
            this.labelInUse.TabIndex = 1;
            this.labelInUse.Text = "Some of the items being deleted are in use by:";
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelWarning.Location = new System.Drawing.Point(21, 9);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(54, 13);
            this.labelWarning.TabIndex = 0;
            this.labelWarning.Text = "Warning";
            // 
            // usages
            // 
            this.usages.Location = new System.Drawing.Point(4, 28);
            this.usages.Multiline = true;
            this.usages.Name = "usages";
            this.usages.ReadOnly = true;
            this.usages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.usages.Size = new System.Drawing.Size(365, 88);
            this.usages.TabIndex = 2;
            // 
            // panelUsages
            // 
            this.panelUsages.Controls.Add(this.labelWarning);
            this.panelUsages.Controls.Add(this.warningIcon);
            this.panelUsages.Controls.Add(this.usages);
            this.panelUsages.Controls.Add(this.labelInUse);
            this.panelUsages.Location = new System.Drawing.Point(10, 49);
            this.panelUsages.Name = "panelUsages";
            this.panelUsages.Size = new System.Drawing.Size(380, 123);
            this.panelUsages.TabIndex = 3;
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
            // DeleteObjectReferenceDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(390, 212);
            this.Controls.Add(this.panelUsages);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteObjectReferenceDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm Delete";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.panelUsages.ResumeLayout(false);
            this.panelUsages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.warningIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelInUse;
        private System.Windows.Forms.Label labelWarning;
        private System.Windows.Forms.TextBox usages;
        private System.Windows.Forms.Panel panelUsages;
        private System.Windows.Forms.PictureBox warningIcon;
    }
}